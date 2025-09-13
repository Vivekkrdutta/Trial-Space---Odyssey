using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraShake : MonoBehaviour
{
    [Tooltip("Controls the overall sensitity of distrurbance")]
    [SerializeField] public float violence = 2f;
    [SerializeField] public float miniTime = 0.2f;
    [SerializeField] public float rotationSensitivity = 1f;
    [SerializeField] float camSpeed = 1f;
    [SerializeField] Transform RoofCamPosition;
    [SerializeField] float GrainFactor = 0.5f;
    PostProcessVolume volume;
    Grain grain;
    float grainFactor;
    float temp;
    [HideInInspector]
    public float rot = 0;
    private void OnEnable()
    {
        grainFactor = GrainFactor;
        transform.localPosition = RoofCamPosition.localPosition;
        transform.localRotation = RoofCamPosition.localRotation;
        timeRec = 0;
    }
    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<Grain>(out grain);
    }
    [HideInInspector]
    public bool breakStreak = false;
    public void Shake()
    {
        ShakeNow = true;
        StartCoroutine(Vibrate());
    }
    IEnumerator Vibrate()
    {
        rot = violence * rotationSensitivity;
        temp = violence;
        while (true)
        {
            yield return new WaitForSecondsRealtime(miniTime);
            if (breakStreak)
                break;
            rot += rotationSensitivity;
            Vector3 dest = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z+temp);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,dest,camSpeed);
            temp *= -1;
        }
        transform.localPosition = RoofCamPosition.localPosition;
        rot = 0;
        temp = 0;
    }
    bool ShakeNow = false;
    float timeRec;
    void Update()
    {
        if(timeRec > 10 || timeRec < 0)
            grainFactor *= -1;
        timeRec += Time.deltaTime * grainFactor;
        grain.lumContrib.Override(timeRec/11);
        if (ShakeNow)
        {
            transform.localRotation = Quaternion.Euler(-rot, 0, 0);
            transform.localRotation = RoofCamPosition.localRotation;
        }
    }
    private void OnDisable()
    {
        breakStreak = false;
        ShakeNow = false;
    }

}
