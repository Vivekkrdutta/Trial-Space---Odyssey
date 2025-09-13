using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickups : MonoBehaviour
{
    [SerializeField] public bool isMedikit = false;
    [Tooltip("value is hp if it is a medikit, coinvalue if it is a coin, or charge if it is a booster.")]
    [SerializeField] public int value;
    Animator animator;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (animator)
            animator.SetTrigger("OnTrig");
    }
}
