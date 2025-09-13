using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneOfItsKInd2 : MonoBehaviour
{
    [SerializeField] int allowedNumber = 1;
    void Awake()
    {
        if( FindObjectsOfType<OnlyOneOfItsKInd2>().Length > allowedNumber)
            Destroy(gameObject);
    }
}
