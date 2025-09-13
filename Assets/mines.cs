using UnityEngine;

public class mines : MonoBehaviour
{
    int myHealth;
    void Start()
    {
        myHealth = GetComponent<Health>().GetHealth();
    }
    [SerializeField] GameObject Blast;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Terrain")
        {
            FindObjectOfType<SecnePersistance>().SpawnMine();
            Destroy(gameObject);
        }
        // Debug.Log("Triggered with Player");
        Instantiate(Blast,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnParticleCollision(GameObject other) {
        myHealth -= other.gameObject.GetComponentInParent<Health>().GetDamage();
        if(myHealth <= 0)
        {
            Instantiate(Blast,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
