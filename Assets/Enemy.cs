using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("movements on death--")]
    [SerializeField] private float dropFactor = 2f;
    [SerializeField] private float frontMoveFactor = 5f;
    [Header("angles changes on death--")]
    [SerializeField] private float rollFactor = 180f;
    [SerializeField] private float rollSensitivity = 20f;
    [SerializeField] private float pitchSensitivity = 30f;
    [Header("Placeholder for the ship")]
    [SerializeField] private GameObject ShipPosition;
    [SerializeField] private int life = 4;
    private bool deathRow = true;
    float pitchCounter = 0;
    float rollCounter = 0;
    void Awake()
    {
        deathRow = false;
    }
    private void OnParticleCollision()
    {
        if (life-- <= 0)
        {
            deathRow = true;
            ShipPosition.GetComponent<Animator>().enabled = false;
            Debug.Log("struck with bullet");
        }
    }
    void Update()
    {
        if (!deathRow)
            return;
        ShipPosition.transform.position = new Vector3(ShipPosition.transform.position.x, ShipPosition.transform.position.y - Time.deltaTime * dropFactor, ShipPosition.transform.position.z);
        if (transform.localRotation.z < rollFactor)
        {
            transform.localRotation = Quaternion.Euler(pitchCounter, 0, rollCounter);
            pitchCounter += Time.deltaTime * pitchSensitivity;
            rollCounter += Time.deltaTime * rollSensitivity;
        }
        transform.localPosition = new Vector3(transform.localPosition.x,
                                                   transform.localPosition.y,
                                                   transform.localPosition.z + Time.deltaTime * frontMoveFactor);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            Debug.Log("struck with ground");
            Destroy(ShipPosition.gameObject);
        }
    }
}
