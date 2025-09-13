using System;
using System.Collections;
using UnityEngine;
public class AnimationControl : MonoBehaviour
{
    [Tooltip("Enter the number of bools, but remember that all strings should be in order!!")]
    [SerializeField] int transitionCount;
    [SerializeField] private Vector2[] times;
    Animator myAnim;
    int pointer = 0;
        string[] heh;

    void Awake()
    {
        myAnim = GetComponent<Animator>();
        heh = new string[transitionCount];
        for (int i = 0; i < transitionCount; i++)
        {
            heh[i] = Convert.ToChar(65+i).ToString();
        }
        StartCoroutine(ApplyAnimations());
    }
    float timeRecords = 0f;
    void Update()
    {
        timeRecords += Time.deltaTime;
    }

    IEnumerator ApplyAnimations()
    {
        while (pointer < transitionCount)
        {
            if (timeRecords >= times[pointer].x)
            {
                myAnim.SetBool(heh[pointer], true);
                yield return new WaitForSecondsRealtime(times[pointer].y - 0.1f);
                myAnim.SetBool(heh[pointer], false);
                pointer++;
                continue;
            }
            else
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
        }
    }

}
