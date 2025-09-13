using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TheCollider : MonoBehaviour
{
    Health health;
    PauseMenu pauseMenu;
    GameObject PlayerShip;
    CameraDamageEffects MCdamageEffects;
    CameraDamageEffects RCdamageEffects;
    theShooter Shooter;
    scoreKeeper scorekeeper;
    void Start()
    {
        health = GetComponentInChildren<Health>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        PlayerShip = health.gameObject;
        Shooter = GetComponent<theShooter>();
        Shooter.MainCamera.gameObject.TryGetComponent<CameraDamageEffects>(out MCdamageEffects);
        Shooter.RoofCamera.gameObject.TryGetComponent<CameraDamageEffects>(out RCdamageEffects);
        scorekeeper = FindObjectOfType<scoreKeeper>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            CameraEffects();
            StartCoroutine(PlayerDeathEffect());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "nextlevel")
            return;
        else if (other.gameObject.tag == "pickups")
        {
            int gain = other.gameObject.GetComponentInParent<pickups>().value;
            Heal();
            health.GetHealth(gain);
            return;
        }
        int dmg = other.gameObject.GetComponentInParent<Health>().GetDamage();
        CameraEffects();
        health.GetHealth(-dmg);
        if (health.currentHealth <= 0.01f)
        {
            StartCoroutine(PlayerDeathEffect());
        }
    }
    int enmdmg;
    bool gotdmgval = false;
    void OnParticleCollision(GameObject other)
    {
        if (!gotdmgval)
        {
            gotdmgval = true;
            enmdmg = other.GetComponentInParent<Health>().GetDamage();
        }
        health.GetHealth(-enmdmg);
        CameraEffects();
        if (health.currentHealth <= 0.01f)
        {
            StartCoroutine(PlayerDeathEffect());
        }
    }
    IEnumerator PlayerDeathEffect()
    {
        {
            Time.timeScale = 0.5f;
            Shooter.DefaultCam(true);
            Shooter.DefaultCanvas(true);
            yield return new WaitForSecondsRealtime(1);
            PlayerShip.SetActive(false);
            Time.timeScale = 0;
        }
        pauseMenu.OnTerranCollision("score :"+scorekeeper.score.ToString()+"\n       Time :"+scorekeeper.timeval.ToString());
    }
    void CameraEffects()
    {
        if (Shooter.mainActive && MCdamageEffects)
            MCdamageEffects.TakeDamage();
        else if (RCdamageEffects)
            RCdamageEffects.TakeDamage();
    }
    void Heal()
    {
        if (Shooter.mainActive && MCdamageEffects)
            MCdamageEffects.Heal();
        else if (RCdamageEffects)
            RCdamageEffects.Heal();
    }
}
