using UnityEngine;
public class playerControls : MonoBehaviour
{
    [Header("Cameras:")]
    [Tooltip("Set up the Primary Camera")]
    [SerializeField] private GameObject MainCam;
    [Tooltip("Set up the Secondary Camera")]
    [SerializeField] private GameObject CabinCam;
    [Header("Movements:")]
    [SerializeField] private float HorizontalVelocity = 1f;
    [SerializeField] private float VerticalVelocity = 1f;
    [SerializeField] private float HorizontalRange = 10f;
    [SerializeField] private float VerticalRange = 10f;
    [SerializeField] private float BalanceFactor = 1f;

    [Header("Rotation:")]
    [SerializeField] float pitchController = 10f;
    [SerializeField] float rollController = 10f;
    [SerializeField] float yawController = 10f;
    [Header("Shooting:")]
    [SerializeField] private ParticleSystem[] Lasers;
    float xThrow;
    float yThrow;
    bool fireOn;
    bool changedCam = false;
    DisableControls paralysis;
    Vector3 Balance;

    void Start()
    {
        CabinCam.SetActive(false);
        paralysis = GetComponent<DisableControls>();
    }

    // Update is called once per frame
    void Update()
    {
        RotationControls();
        CameraControls();
        FireControls();
        if (paralysis.GetDisability())
        {
            RestoreBalance();
            return;
        }
        xThrow = Input.GetAxis("Horizontal") * HorizontalVelocity * Time.deltaTime;
        yThrow = Input.GetAxis("Vertical") * VerticalVelocity * Time.deltaTime;
        MovementControls();
    }

    private void FireControls()
    {
        foreach (ParticleSystem gun in Lasers)
        {
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetMouseButtonDown(0))
            {
                gun.Play();
            }
            if (Input.GetKeyUp(KeyCode.RightShift) || Input.GetMouseButtonUp(0))
            {
                gun.Stop();
            }
        }
    }
    private void CameraControls()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!changedCam)
            {
                changedCam = true;
                MainCam.SetActive(false);
                CabinCam.SetActive(true);
            }
            else
            {
                changedCam = false;
                MainCam.SetActive(true);
                CabinCam.SetActive(false);
            }
        }

    }
    private void MovementControls()
    {
        transform.localPosition = new Vector3
                                      (Mathf.Clamp(transform.localPosition.x + xThrow, -HorizontalRange, HorizontalRange),
                                      Mathf.Clamp(transform.localPosition.y + yThrow, -VerticalRange + 1f, VerticalRange - 1f),
                                      transform.localPosition.z);
    }

    private void RotationControls()
    {
        float pitch = transform.localPosition.y * pitchController;
        float yaw = transform.localPosition.x * yawController;
        float roll = transform.localPosition.x * rollController;
        transform.localRotation = Quaternion.Euler
                                    (pitch,
                                    yaw,
                                    roll);
    }
    public void RestoreBalance()
    {
        if (Check_BnO())
        {
            Resume();
            return;
        }
        Balance = transform.localPosition;
        Balance = new Vector3(
            Balance.x - Bal_Fact(Balance.x),
            Balance.y - Bal_Fact(Balance.y),
            Balance.z - Bal_Fact(Balance.z));

        transform.localPosition = Balance;
    }

    // Other Utility Functions:


    private float Bal_Fact(float val)
    {
        return val > 0 ? Time.deltaTime*BalanceFactor : val < 0 ? -Time.deltaTime*BalanceFactor : 0;
    }

    private bool Check_BnO()
    {
        if (AtAlmostZero(transform.localPosition.x)
            && AtAlmostZero(transform.localPosition.y)
           )
        {
            SetBalanceFactor(1);
            return true;
        }
        return false;
    }
    bool AtAlmostZero(float val)
    {
        if (val <= 0.2 && val >= -0.2)
            return true;
        return false;
    }
    void Resume()
    {
        paralysis.SetDisability(false);
        Debug.Log("freed!!");
    }
    public float GetBalanceFactor()
    {
        return BalanceFactor;
    }
    public void SetBalanceFactor(float val)
    {
        BalanceFactor = val;
    }
}
