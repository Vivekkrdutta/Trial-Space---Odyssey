using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
// using UnityEditor.Rendering.LookDev;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] bool VariablesAreEffective = true;
    bool isPuase = false;
    GameObject PauseCamera;
    theShooter shooter;
    [SerializeField] public Image PlayerHealthimage;
    [SerializeField] TextMeshProUGUI CentralText;
    [Tooltip("End Screen Black Panel")]
    [SerializeField] GameObject EndScreenPanel;
    [SerializeField] GameObject HealthSidePanel;
    [SerializeField] Button ResumeButton;
    GameManager gameManager;
    // int MaxHealth;
    void Awake()
    {
        EndScreenPanel.SetActive(false);
        // HealthSidePanel.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        if (VariablesAreEffective)
        {
            PauseCamera = FindObjectOfType<theMover>().GetComponentInChildren<OnlyOneOfItsKind>().gameObject;
            PauseCamera.SetActive(false);
            shooter = FindObjectOfType<theShooter>();
        }
        DominateText("");
        Time.timeScale = 0.5f;
        StartCoroutine(CountDown(5, 1));
    }
    string tempText;
    public void Puase(string val = "PAUSE")
    {
        isPuase = !isPuase;
        if (isPuase)
        {
            EndScreenPanel.SetActive(true);
            tempText = CentralText.text;
            DominateText(val);
            Time.timeScale = 0;
            shooter.SetBothCamera(false);
            HealthSidePanel.SetActive(false);
            PauseCamera.SetActive(true);
        }
        else
        {
            PauseCamera.SetActive(false);
            HealthSidePanel.SetActive(true);
            EndScreenPanel.SetActive(false);
            CentralText.text = tempText;
            float tempTime = Time.timeScale;
            StartCoroutine(CountDown(3, 0.25f, tempTime));
            shooter.SetBothCamera(true);
        }
    }

    public void DominateText(string val)
    {
        CentralText.text = val;
    }
    public void DominateText(string firstVal, float time)
    {
        // Debug.Log("Working");
        CentralText.text = firstVal;
        StartCoroutine(Wait(time));
    }
    IEnumerator CountDown(int count, float time, float ScaleValue = 1)
    {
        for (int i = count; i > 0; i--)
        {
            CentralText.text = i.ToString();
            yield return new WaitForSecondsRealtime(time);
        }
        CentralText.text = "GO!";
        Time.timeScale = ScaleValue;
        StartCoroutine(Wait(time));
    }
    IEnumerator Wait(float val)
    {
        yield return new WaitForSecondsRealtime(val);
        CentralText.text = "";
        HealthSidePanel.SetActive(true);
    }

    public void ReTry()
    {
        // FindObjectOfType<SecnePersistance>().ReloadLevel(1);
        StartCoroutine(gameManager.SceneDelay(gameManager.LoadDelay, 1, false));
        // this.gameObject.SetActive(false);
    }
    public void QuitToMainMenu()
    {
        StartCoroutine(gameManager.SceneDelay(gameManager.LoadDelay, 0, true));
        // this.gameObject.SetActive(false);

    }
    public void OnTerranCollision(string val = "")
    {
        EndScreenPanel.SetActive(true);
        ResumeButton.interactable = false;
        tempText = CentralText.text;
        DominateText(val);
        Time.timeScale = 0;
        shooter.SetBothCamera(false);
        HealthSidePanel.SetActive(false);
        PauseCamera.SetActive(true);
    }
}
