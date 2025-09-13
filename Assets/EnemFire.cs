using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftLaser;
    [SerializeField] private ParticleSystem rightLaser;
    Enem_Movement Parent;
    bool firing = false;
    // bool attacker = false;
    void Awake()
    {
        Parent = GetComponentInParent<Enem_Movement>();
        // attacker = Parent.isAnAttacker;
        // if(!attacker)
        // {
        //     leftLaser.Play();
        //     rightLaser.Play();
        // }
    }
    void Update()
    {
        if (!Parent.Problem && !firing && Parent.isAnAttacker)
        {
            firing = true;
            // Debug.Log("firing");
            leftLaser.Play();
            rightLaser.Play();
        }
        else if ((Parent.Problem || !Parent.isAnAttacker) && firing)
        {
            firing = false;
            // Debug.Log("NOt firing");
            leftLaser.Stop();
            rightLaser.Stop();
        }
    }
    bool isBetween(float val, float lowerval, float upperval)
    {
        return val < upperval && val > lowerval;
    }
}
