using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class theMover : MonoBehaviour
{
    [Header("For the Holder")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float holderUpVelocity = 0.1f;
    [SerializeField] float holderRightVelocity = 0.1f;
    [SerializeField]Vector2 Range;
    [SerializeField] float additonalHeight = 100f;

    [Header("For the ship")]
    [SerializeField] float shipUpVelocity = 1f;
    [SerializeField] float shipRightVelocity = 1f;
    [SerializeField] private GameObject Ship;
    [SerializeField] Vector3 rotationVector;
    [SerializeField] Vector2 ShipRange;

    [Header("For the Cameras")]
    [SerializeField] Transform MainCamera;
    [SerializeField] Vector2 CameraRange;
    float cameraUpVelocity = 12f;
    float cameraRightVelocity = 12f;
    float vertz, horz;
    CameraShake shaker;

    public void Setup()
    {
        moveSpeed += 5f;
        shipRightVelocity += 3f;
        shipUpVelocity += 3f;
        cameraUpVelocity += 3f;
        cameraRightVelocity += 3f;
    }
    void Start()
    {
        cameraRightVelocity = CameraRange.x / Range.x * holderRightVelocity;
        cameraUpVelocity = CameraRange.y / Range.y * holderUpVelocity;
        shaker = GetComponent<theShooter>().Shaker;
    }

    void Update()
    {
        horz = Input.GetAxis("Horizontal");
        vertz = Input.GetAxis("Vertical");
        MovementControls();
        RotationControls();
    }
    private void MovementControls()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveSpeed * Time.deltaTime);

        Ship.transform.localPosition = new Vector3(
            Mathf.Clamp(Ship.transform.localPosition.x + horz * shipRightVelocity * Time.deltaTime, -ShipRange.x, ShipRange.x),
            Mathf.Clamp(Ship.transform.localPosition.y + vertz * shipUpVelocity * Time.deltaTime, -ShipRange.y, ShipRange.y),
            Ship.transform.localPosition.z);

        Vector3 shipsPosition = Ship.transform.localPosition;
        if (shipsPosition.x >= ShipRange.x - 0.5f || shipsPosition.x <= -ShipRange.x + 0.5f)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x + horz * holderRightVelocity * Time.deltaTime, -Range.x, Range.x),
                transform.position.y,
                transform.position.z
            );

            MainCamera.transform.localPosition = new Vector3(
                Mathf.Clamp(MainCamera.transform.localPosition.x + horz * cameraRightVelocity * Time.deltaTime, -CameraRange.x, CameraRange.x),
                MainCamera.transform.localPosition.y,
                MainCamera.transform.localPosition.z
            );
        }
        if (shipsPosition.y >= ShipRange.y - 0.4f || shipsPosition.y <= -ShipRange.y + 0.4f)
        {
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y + vertz * holderUpVelocity * Time.deltaTime, -Range.y + additonalHeight, Range.y + additonalHeight),
                transform.position.z
            );

            MainCamera.transform.localPosition = new Vector3(
                MainCamera.transform.localPosition.x,
                Mathf.Clamp(MainCamera.transform.localPosition.y + vertz * cameraUpVelocity * Time.deltaTime, -CameraRange.y, CameraRange.y),
                MainCamera.transform.localPosition.z
            );
        }
    }
    private bool shouldShake;
    public void alterShakepermission(bool val)
    {
        shouldShake = val;
        if (shouldShake)
            StartCoroutine(Vibrate());
    }
    private void RotationControls()
    {
        Ship.transform.localRotation = Quaternion.Euler(
                                    rotationVector.x * vertz - shakerot,
                                    rotationVector.y * horz,
                                    rotationVector.z * horz
        );
    }
    float shakerot;
    IEnumerator Vibrate()
    {
        float rotsen = shaker.rotationSensitivity;
        shakerot = shaker.violence * shaker.rotationSensitivity;
        float miniTime = shaker.miniTime;
        while (true)
        {
            yield return new WaitForSecondsRealtime(miniTime);
            if (!shouldShake)
                break;
            shakerot += rotsen;
        }
        shakerot = 0;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public Vector3 GetShipPosition()
    {
        return Ship.transform.position;
    }
    public Transform GetShipTransform()
    {
        return Ship.transform;
    }
    ~theMover() { }

}
