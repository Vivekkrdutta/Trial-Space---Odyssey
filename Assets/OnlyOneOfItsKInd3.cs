using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyOneOfItsKInd3 : MonoBehaviour
{
    [SerializeField] int allowedNumber = 1;
    void Awake()
    {
        if( FindObjectsOfType<OnlyOneOfItsKInd3>().Length > allowedNumber)
            Destroy(gameObject);
    }
}
