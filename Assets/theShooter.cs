using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class theShooter : MonoBehaviour
{
    [SerializeField] public ParticleSystem LeftSocket;
    [SerializeField] public ParticleSystem RightSocket;
    [SerializeField] public GameObject MainCamera;
    [SerializeField] public GameObject RoofCamera;
    [SerializeField] float DelayInstance = 2f;

    [Header("The firing variables :")]
    [Tooltip("denotes the time for which the rage meter can last")]
    [SerializeField] int FireTime = 5;
    [SerializeField] int CoolDownTime = 3;
    [Tooltip("x denotes the falling speed of the rage meter. y denotes the rising value.")]
    [SerializeField] Vector2 SlowFactor;
    private bool changedCam = true;
    private float firetime;
    [SerializeField] UIHandler MainUihandler;
    [SerializeField] GameObject RoofUihandler;
    [SerializeField] PauseMenu CentralCanvas;
    PauseMenu centralCanvas;
    UIHandler roofHandler;
    public CameraShake Shaker;
    theMover mover;
    void Awake()
    {
        RoofUihandler = Instantiate(RoofUihandler);
        roofHandler = RoofUihandler.GetComponent<UIHandler>();
        centralCanvas = Instantiate(CentralCanvas);
        StartCoroutine(InstanceDelayer(DelayInstance));
        Shaker = RoofCamera.GetComponent<CameraShake>();
        mover = GetComponent<theMover>();
    }
    void Start()
    {
        firetime = FireTime;
        DefaultCam(true);
        DefaultCanvas(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            centralCanvas.Puase();
        if (Time.timeScale < 1)
            return;
        ControlFireSlider();
        if (firetime > 0 && (Input.GetKey(KeyCode.F) || Input.GetMouseButton(0)))
        {
            firetime -= Time.deltaTime * SlowFactor.x;
            if (firetime <= 0 && LeftSocket.isPlaying)
            {
                LeftSocket.Stop();
                RightSocket.Stop();
                centralCanvas.DominateText("BARREL EMPTY !", 3);
            }
        }
        CamControls();
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            if (firetime > 0)
            {
                LeftSocket.Play();
                RightSocket.Play();
                Firing = true;
            }
            mover.alterShakepermission(true);
            if (!mainActive)
            {
                Shaker.Shake(); // the stopping of this coroutine is defined by another variable called breakstreak, controlled by DefaultCam();
            }
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonUp(0))
        {
            LeftSocket.Stop();
            RightSocket.Stop();
            if (functionCount == 0)
                StartCoroutine(Restore());
            Firing = false;
            mover.alterShakepermission(false);
            if (!mainActive)
            {
                Shaker.breakStreak = true;
            }
        }
    }
    IEnumerator InstanceDelayer(float val)
    {
        CanvasGroup y = RoofUihandler.GetComponent<CanvasGroup>();
        CanvasGroup x = centralCanvas.GetComponent<CanvasGroup>();
        y.alpha = 0;
        x.alpha = 0;
        yield return new WaitForSecondsRealtime(val);
        x.alpha = 1;
        y.alpha = 1;
    }
    bool Firing = false;
    public bool GetFiringSts()
    {
        return Firing;
    }
    public bool mainActive = true;
    public void DefaultCam(bool val)
    {
        changedCam = val;
        Shaker.breakStreak = val;
        MainCamera.SetActive(changedCam);
        RoofCamera.SetActive(!changedCam);
    }
    public void DefaultCanvas(bool val)
    {
        mainActive = val;
        MainUihandler.gameObject.SetActive(val);
        RoofUihandler.SetActive(!val);
    }
    int functionCount = 0; // so that a coroutine runs only once, and not many times.
    IEnumerator Restore()
    {
        functionCount++;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSecondsRealtime(CoolDownTime);
        while (firetime < FireTime && !Firing)
        {
            yield return new WaitForEndOfFrame();
            firetime += Time.deltaTime * SlowFactor.y;
        }
        functionCount--;
    }
    private void ControlFireSlider()
    {
        if (mainActive)
            MainUihandler.ChangeSlider(firetime / FireTime);
        else
            roofHandler.ChangeSlider(firetime / FireTime);
    }

    public void SetBothCamera(bool val)
    {
        // RoofCamera.SetActive(false);
        MainCamera.SetActive(val);
    }

    private void CamControls()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DefaultCam(false);
            DefaultCanvas(false);
            if (Firing)
            {
                Shaker.Shake();
                // mover.alterShakepermission(true);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            DefaultCam(true);
            DefaultCanvas(true);
            // mover.alterShakepermission(false);
        }
    }
}
