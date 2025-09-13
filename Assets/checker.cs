using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checker : MonoBehaviour
{
    // bool attacker;
    Enem_Movement Parent;
    void Start()
    {
        Parent = GetComponentInParent<Enem_Movement>();
        // attacker = Parent.isAnAttacker;
        // if(Parent.isAnAttacker)
        // {
        //     SetRoam(false);
        //     SetProblem(false);
        // }
        // else
        //     SetProblem(true);
        SetProblem(false);
    }
    private void OnTriggerEnter(Collider other) {
        // if(Parent.isAnAttacker)
        // {
        //     SetProblem(true);
        //     // SetRoam(false);
        // }
        // else
        // {
        //     SetRoam(false);
        //     // SetProblem()
        // }
        // Debug.Log(" problem !!");
        SetProblem(true);
    }
    private void OnTriggerExit(Collider other) {
        // if(Parent.isAnAttacker)
        //     SetProblem(false);
        // else 
        //     SetRoam(true);
        // // Debug.Log("No problem.");
        SetProblem(false);
    }
    void SetProblem(bool val)
    {
        Parent.Problem = val;
    }
    void SetRoam(bool val)
    {
        Parent.Roam = val;
    }
}
