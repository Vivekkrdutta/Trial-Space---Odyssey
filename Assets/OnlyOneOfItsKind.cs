using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneOfItsKind : MonoBehaviour
{
    [SerializeField] int allowedNumber = 1;
    void Awake()
    {
        if( FindObjectsOfType<OnlyOneOfItsKind>().Length > allowedNumber)
            Destroy(gameObject);
    }
}
