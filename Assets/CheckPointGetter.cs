using UnityEngine;

public class CheckPointGetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "nextlevel")
        {
            FindObjectOfType<SecnePersistance>().InstantiateLand();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "nextlevel")
        {
            other.gameObject.GetComponentInParent<autoDestroy>().TriggerDestroy();
        }
    }
}
