using UnityEngine;

public class DisableControls : MonoBehaviour
{
    float timeRecords = 0f;
    static int thisTimestamp = 0;

    [SerializeField] float[] TimeStamps;
    // Start is called before the first frame update
    private bool disableControls = false;
    void Start()
    {
        thisTimestamp = 0;
        timeRecords = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeRecords += Time.deltaTime;
        ControlDisable();
    }
    void ControlDisable()
    {
        if(thisTimestamp >= TimeStamps.Length)
            return;
        if (timeRecords >= TimeStamps[thisTimestamp] - 0.1 && timeRecords <= TimeStamps[thisTimestamp] + 0.1)
        {
            disableControls = true;
            Debug.Log("Disabled!");
            thisTimestamp++;
        }
    }
    public bool GetDisability()
    {
        return disableControls;
    }
    public void SetDisability(bool val)
    {
        disableControls = val;
    }
}
