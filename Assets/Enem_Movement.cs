using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enem_Movement : MonoBehaviour
{
    [Header("Detectors")]

    [Tooltip("Enter all the detection colliders serial wise")]
    [SerializeField] private List<GameObject> DetectionItems;
    [SerializeField] private Transform Ship;

    [Header("Variables")]

    // [Tooltip("This value must match with the player's front move-velocity")]
    private float moveSpeed;

    [Tooltip("The speed at which it will normally move compared to the Player,after normalisation.")]
    [Range(1, 2)][SerializeField] private float speedMulitplier = 1.2f;

    [Tooltip("Will determine the intelligence and perfomance of the Enemy ship")]
    [SerializeField] private float waitTime = 1f;

    [Tooltip("For instancing only")]
    [SerializeField] private Vector3 Range; // for instantiating
    [SerializeField] private Vector3 RotationVector;

    [Tooltip("Distance at which the enemy goes normal, between x and y")]
    [SerializeField] private Vector2 normallingDistance;

    [Tooltip("will destroy itself when the enemy is at this distance from the player")]
    [SerializeField] private float destroyAtDistance = 100f;

    [Tooltip("Pre-built height")]
    [SerializeField] private float additionalhight = 90f;

    [Tooltip("The speed with which it will fall at the time of death")]
    [SerializeField] private float fallSpeed = 15f;

    [Tooltip("What the f*** does the name suggests? huh?")]
    [SerializeField] private float fallFrontSpeed = 10f;
    [Tooltip("Randomly choses a velocity b/w the given x and y (b/w their postive and negative values.)")]
    [SerializeField] private Vector2 freeRoamVelocity;

    [Tooltip("Randomly changes free roam destination point at a random time b/w x and y")]
    [SerializeField] private Vector2 freeRoamChangeTime;

    [Header("Attacks")]
    [SerializeField] public bool isAnAttacker = true;
    [SerializeField] private float attackSpeed;

    [Tooltip("Duration for which the ship remains in front of the Player for attacking him, after normalisation.")]
    [SerializeField] private int normalTime = 10;

    [Tooltip("How fast the Enemy changes its position for attacking the Player")]
    [SerializeField] private Vector2 slowFactor;
    [SerializeField] public int pointsToPlayer = 100;
    [HideInInspector]
    public bool Problem = false;
    [HideInInspector]
    public bool Roam = true;
    [HideInInspector]
    public theMover thePlayer;
    Vector2 Velocity;
    List<Enem_Alert> Stack;
    float originalMoveSpeed;
    int top = 0;
    private int normalAtDistance;
    AttackShifter atksftr;
    int EnmCount = 0;
    bool imCreatedBro = false;
    void Awake()
    {
        if (FindObjectsOfType<Enem_Movement>().Length > FindObjectOfType<SecnePersistance>().GetAllowedNumber())
        {
            Destroy(gameObject);
            ProcessDeath(0);
        }
        thePlayer = FindObjectOfType<theMover>();
        // checkerO = GetComponentInChildren<checker>();
        transform.position = new Vector3(
            Random.Range(-Range.x, Range.x),
            Random.Range(additionalhight - Range.y, additionalhight + Range.y),
            Random.Range(thePlayer.transform.position.z - Range.z / 2, thePlayer.transform.position.z - Range.z / 3)
        );
        moveSpeed = thePlayer.GetMoveSpeed();
        originalMoveSpeed = moveSpeed;
        moveSpeed *= 2;
        AttackerUpdate();
    }
    void AttackerUpdate()
    {
        atksftr = FindObjectOfType<AttackShifter>();
        atksftr.ActiveEnemies.Add(this);
        EnmCount = atksftr.ActiveEnemies.Count - 1;
        imCreatedBro = true;
    }
    void Start()
    {
        Velocity = new Vector2(0, 0);
        Stack = new List<Enem_Alert>(DetectionItems.Count);
        foreach (GameObject child in DetectionItems)
        {
            Stack.Add(child.GetComponent<Enem_Alert>());
        }
        StartCoroutine(SetPosition());
        // if (!isAnAttacker)
        StartCoroutine(FreeRoam());
        normalAtDistance = (int)Random.Range(normallingDistance.x, normallingDistance.y);
        StartCoroutine(TriggerNormal());
    }
    private bool triggerNormal = false;
    private bool vergeOfDestruct = false;
    private float timeRec = 0f;
    void Update()
    {
        if (vergeOfDestruct)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - fallSpeed * Time.deltaTime,
                transform.position.z + fallFrontSpeed * Time.deltaTime
            );
            transform.rotation = Quaternion.identity;
            return;
        }
        if (!triggerNormal) // check for normalling trigger.
        {
            timeRec += Time.deltaTime;
            if (timeRec >= 10f) // Case when the enemy is stuck in the terrain and does not behave as expected (i.e. if the enemy does not pass the player ship under at most 10 seconds, the ship is probably stuct), destroy it.
            {
                ProcessDeath(0);
            }
        }

        transform.position = new Vector3(
            transform.position.x + Velocity.x * Time.deltaTime,
            transform.position.y + Velocity.y * Time.deltaTime,
            transform.position.z + moveSpeed * Time.deltaTime
        );
        Ship.localRotation = Quaternion.Euler(
            Velocity.y * RotationVector.x,
            Velocity.x * RotationVector.y,
            Velocity.x * RotationVector.z
        );
    }
    IEnumerator DestroyAtDistance()
    {
        yield return new WaitForSecondsRealtime(normalTime);
        moveSpeed *= speedMulitplier;
        if(isAnAttacker)
        {
            isAnAttacker = false;
            atksftr.leaderIsThere = false;
        }
        yield return new WaitForSecondsRealtime(destroyAtDistance / (moveSpeed - originalMoveSpeed));
        ProcessDeath(0);
    }
    IEnumerator FreeRoam()
    {
        while (true)
        {
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            float tempTime = Random.Range(freeRoamChangeTime.x, freeRoamChangeTime.y);
            yield return new WaitForSecondsRealtime(tempTime);
            if(Problem && isAnAttacker)
            {
                isAnAttacker = false;
                atksftr.leaderIsThere = false;
            }
            if (Problem) //* !Roam in Problem
                continue;
            Velocity.x = Random.Range(-freeRoamVelocity.x, freeRoamVelocity.x);
            Velocity.y = Random.Range(-freeRoamVelocity.y, freeRoamVelocity.y);
        }
    }

    IEnumerator SetPosition()
    {
        while (true)
        {
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            if (transform.position.x < -40f || transform.position.x > 40f)
            {
                ProcessDeath(0);
            }
            else if (transform.position.y > additionalhight + 16f || transform.position.y < additionalhight - 20f)
            {
                ProcessDeath(0);
            }
            if (vergeOfDestruct)
            {
                ProcessDeath(0.2f);
                break;
            }
            yield return new WaitForSecondsRealtime(waitTime);
            if (isAnAttacker && !Problem) // !problem and Triggernormal
            {
                Vector3 AtkDestination = thePlayer.GetShipPosition();
                Velocity.x = (AtkDestination.x - transform.position.x) * slowFactor.x;
                Velocity.y = (AtkDestination.y - transform.position.y) * slowFactor.y;
            }
            else if (Problem) //!Roam
            {
                while (!Stack[top].GetAvailable())
                {
                    top++;
                    if (top > Stack.Count - 1)
                    {
                        top = 0;
                        break;
                    }
                }
                Vector3 Destination = DetectionItems[top].transform.position;
                float timeRec = (Destination.z - transform.position.z) / moveSpeed;
                Velocity.x = (Destination.x - transform.position.x) / timeRec;
                Velocity.y = (Destination.y - transform.position.y) / timeRec;
                top = 0;
            }
        }
    }
    private void ProcessDeath(float t)
    {
        if (t == 0)
            Destroy(gameObject);
        StartCoroutine(killhim(t));
    }
    IEnumerator killhim(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        Destroy(gameObject);
    }
    IEnumerator TriggerNormal()
    {
        float time = (thePlayer.transform.position.z + normalAtDistance - transform.position.z) / (moveSpeed - originalMoveSpeed);
        while (time > 0)
        {
            yield return new WaitForSecondsRealtime(2);
            if (Time.timeScale >= 1)
                time -= 2;
        }
        triggerNormal = true;
        moveSpeed = originalMoveSpeed;
        if (speedMulitplier > 1)
            StartCoroutine(DestroyAtDistance());
    }
    public void SetVergeOfDestruct(bool val)
    {
        vergeOfDestruct = val;
    }
    public bool GetTriggerNormal()
    {
        return triggerNormal;
    }
    public GameObject GetShip()
    {
        return Ship.gameObject;
    }
    private void OnDestroy()
    {
        // print(EnmCount);
        if (imCreatedBro)
            atksftr.ActiveEnemies.Remove(this);
        if (isAnAttacker)
        {
            isAnAttacker = false;
            atksftr.leaderIsThere = false;  
        }
    }

}
