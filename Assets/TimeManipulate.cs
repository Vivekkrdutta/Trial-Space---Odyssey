using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulate : MonoBehaviour
{
    [SerializeField] float waitTime = 20;
    theMover mover;
    SecnePersistance persistance;
    void Start()
    {
        mover = FindObjectOfType<theMover>();
        persistance = FindObjectOfType<SecnePersistance>();
    }
    IEnumerator IncreaseValues()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            mover.Setup();
            persistance.Setup();
        }
    }
}
