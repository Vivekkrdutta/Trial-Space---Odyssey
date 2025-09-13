using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public float LoadDelay = 2f;
    [SerializeField] Image CentralPanel;
    public GameObject Maincamera;
    [SerializeField] float RotateSpeed = 3f;
    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
        Screen.SetResolution(1240,500,false);
        Maincamera = FindObjectOfType<Camera>().gameObject;
    }
    public void Play()
    {
        StartCoroutine(SceneDelay(LoadDelay, 1));
    }
    public IEnumerator SceneDelay(float val, int scene, bool toMainMenu = false)
    {
        Time.timeScale = 1;
        animator.SetTrigger("Trig");
        yield return new WaitForSecondsRealtime(val);
        Rotate = toMainMenu;
        SceneManager.LoadScene(scene);
        if (toMainMenu)
            Maincamera = FindObjectOfType<Camera>().gameObject;
        CentralPanel.gameObject.SetActive(toMainMenu);
    }
    public bool Rotate = true;
    void Update()
    {
        if (Rotate == true && Maincamera)
            Maincamera.transform.Rotate(0,RotateSpeed * Time.deltaTime,0);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
