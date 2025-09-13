using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scoreKeeper : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ScoreText;
    [SerializeField] public TextMeshProUGUI TimePlayedText;
    void Start()
    {
        ScoreText.text = "000000";
        TimePlayedText.text = "";
        StartCoroutine(updateTime());
    }
    [HideInInspector]
    public
    int score = 0;
    public void UpdateScore(int val)
    {
        score += val;
        ScoreText.text = score.ToString("000000");
    }
    [HideInInspector]
    public
    float timeval = 0;
    IEnumerator updateTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            if (Time.timeScale > 0)
            {
                TimePlayedText.text = timeval.ToString("000000");
                timeval += 1;
            }
        }
    }
}
