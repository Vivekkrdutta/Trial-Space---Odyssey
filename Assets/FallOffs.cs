// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FallOffs : MonoBehaviour
{
    [SerializeField] float yInstantiate = 130f;
    [SerializeField] float zInstantiate = 300f;
    [SerializeField] float zDestination;
    [SerializeField] float DestinationRange = 20f;
    [SerializeField] float BehindDestroyDistance = 30f;
    [SerializeField] int killTime = 0;
    Vector3 ship;
    float moveSpeed;
    private theMover thePlayer;
    Vector3 Destination;
    float xVel;
    float yVel;
    float distZ;
    int health;
    void Awake()
    {
        health = GetComponent<Health>().GetHealth();
        thePlayer = FindObjectOfType<theMover>();
        ship = thePlayer.GetShipPosition();
        transform.position = new Vector3(
            Random.Range(-DestinationRange, DestinationRange),
            yInstantiate,
            thePlayer.transform.position.z + zInstantiate
        );
        Destination = new Vector3(
            Random.Range(-DestinationRange, DestinationRange),
            ship.y,
            thePlayer.transform.position.z + zDestination
        );
        float distX = Destination.x - transform.position.x;
        float distY = Destination.y - transform.position.y;
        distZ = Destination.z - transform.position.z;

        moveSpeed = thePlayer.GetMoveSpeed() * distZ / zDestination;
        xVel = distX / distZ * moveSpeed;
        yVel = distY / distZ * moveSpeed;

    }

    void Update()
    {
        if (transform.position.z <= Destination.z - BehindDestroyDistance)
        {
            Destroy(gameObject);
        }
        transform.Translate(xVel * Time.deltaTime,
                            yVel * Time.deltaTime,
                            moveSpeed * Time.deltaTime);

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
            StartCoroutine(ProcessDeath(killTime));
        // else
        // {

        //     health -= other.gameObject.GetComponent<Health>().GetDamage();
        //     if(health <= 0)
        //         StartCoroutine(ProcessDeath(0));
        // }
    }
    private void OnTriggerEnter(Collider other) {
        StartCoroutine(ProcessDeath(0));
    }
    private void OnParticleCollision(GameObject other) {
        // Debug.Log("Meteor was hit with bullet");
        health -= other.gameObject.GetComponentInParent<Health>().GetDamage();
        if(health <= 0)
            StartCoroutine(ProcessDeath(0));
    }

    IEnumerator ProcessDeath(int val)
    {
        Instantiate(FindObjectOfType<SecnePersistance>().MeteorBlast,transform.position,Quaternion.identity);
        yield return new WaitForSecondsRealtime(val);
        Destroy(gameObject);
    }

}
