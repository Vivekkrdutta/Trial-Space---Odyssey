using System;
using System.Collections;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    DisableControls paralysis;
    playerControls playercontroller;
    private void Awake()
    {
        paralysis = GetComponent<DisableControls>();
        playercontroller = GetComponent<playerControls>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            Debug.Log("jet is Touching the Terrain!!");
            playercontroller.SetBalanceFactor(2);
            paralysis.SetDisability(true);
        }
    }
}
