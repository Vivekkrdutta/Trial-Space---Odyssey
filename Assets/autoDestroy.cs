using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoDestroy : MonoBehaviour
{
    [SerializeField] bool DestroyByTrigger = false;
    [SerializeField] bool DestroyByDefault = false;
    [SerializeField] int DestroyTime = 5;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject, DestroyTime);
    }
    void Start()
    {
        if (DestroyByDefault)
            Destroy(gameObject, DestroyTime);
    }

    public void TriggerDestroy()
    {
        if (DestroyByTrigger)
            Destroy(gameObject, DestroyTime);
    }
}
