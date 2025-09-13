using UnityEngine;
public class Enem_Collision : MonoBehaviour
{
    Health health;
    scoreKeeper ScoreKeeper;
    int pointsToPlayer;
    void Start()
    {
        health = GetComponent<Health>();
        ScoreKeeper = FindObjectOfType<scoreKeeper>();
        pointsToPlayer = GetComponentInParent<Enem_Movement>().pointsToPlayer;
    }
    [SerializeField] GameObject Explostion;
    [SerializeField] GameObject BigExplosion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain" && GetComponentInParent<Enem_Movement>().GetTriggerNormal())
        {
            Instantiate(BigExplosion, transform.position, Quaternion.identity);
            GetComponentInParent<Enem_Movement>().SetVergeOfDestruct(true);
            // Debug.Log("Terrained");
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        int dmg = other.gameObject.GetComponentInParent<Health>().GetDamage();
        Transform mytransform = transform;
        Instantiate(Explostion, mytransform);
        int currentHealth = health.GetHealth(-dmg);
        if (currentHealth <= 0)
        {
            Instantiate(BigExplosion, transform.position, Quaternion.identity);
            GetComponentInParent<Enem_Movement>().SetVergeOfDestruct(true);
            ScoreKeeper.UpdateScore(pointsToPlayer);
        }
    }
}
