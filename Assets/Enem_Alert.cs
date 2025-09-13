using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enem_Alert : MonoBehaviour
{
    [SerializeField] Vector2 Range;
    [SerializeField] float additonalHeight = 100f;
    private bool available;
    void Awake()
    {
        available = true;
    }
    private void OnTriggerEnter(Collider other)
    {
            available = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (
            transform.position.x >= Range.x ||
            transform.position.x <= -Range.x ||
            transform.position.y >= additonalHeight + Range.y ||
            transform.position.y <= additonalHeight - Range.y
        )
            available = false;
        else
            available = true;
    }
    public bool GetAvailable()
    {
        return available;
    }
}
