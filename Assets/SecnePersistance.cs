using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecnePersistance : MonoBehaviour
{
    [SerializeField] theMover Player;

    [Header("Meteors")]
    [SerializeField] bool activateMeteors = true;
    [SerializeField] List<GameObject> Meteors;
    [SerializeField] float MeteorSpawnTime = 10f;
    [SerializeField] float MeteorSpawnTimeVariance = 5f;
    [SerializeField] int NumberOfMeteorSpawns = 1;
    [SerializeField] public GameObject MeteorBlast;
    [Header("Enemy ships")]
    [SerializeField] bool activateEnemies = true;
    [SerializeField] List<GameObject> Enemies;
    [SerializeField] float EnemySpawnTime = 20f;
    [SerializeField] float EnemySpawnTimeVariance = 5f;
    [SerializeField] Vector2 IndiVidualSpawn;
    [SerializeField] int NumberOfEnemySpawns = 2;
    [SerializeField] private int allowedNumber = 3;
    [Header("The Mines")]
    [SerializeField] bool activateMines = true;
    [SerializeField] GameObject Mine;

    [Tooltip("Spawns a random mine between x and y")]
    [SerializeField] Vector2 MineSpawnTime;
    [SerializeField] Vector2 MineIndividualSpawn;
    [SerializeField] int NumberOfMineSpawns = 3;
    // [SerializeField] int maxMines = 5;
    [Tooltip("For Instancing Only, horizontal range b/w -x and x, z axis range : b/w values of y and z")]
    [SerializeField] Vector3 Range;

    [Header("The Lands")]
    [SerializeField] public GameObject Land;
    [SerializeField] public float landInstantiateDistanceZ = 5000f;

    AttackShifter atksftr;
    void Awake()
    {
        Player = Instantiate(Player);
        atksftr = GetComponent<AttackShifter>();
    }
    void Start()
    {
        if (activateMeteors)
            StartCoroutine(MeteorSpawn());
        if (activateEnemies)
            StartCoroutine(EnemySpawn());
        if (activateMines)
            StartCoroutine(MineSpawn());
    }
    IEnumerator MeteorSpawn()
    {
        while (true)
        {
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            yield return new WaitForSecondsRealtime
                            (Random.Range(MeteorSpawnTime - MeteorSpawnTimeVariance,
                            MeteorSpawnTime + MeteorSpawnTimeVariance));
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            for (int i = 0; i < NumberOfMeteorSpawns; i++)
            {
                Instantiate(Meteors[Random.Range(0, Meteors.Count)]);
                yield return new WaitForSecondsRealtime(Random.Range(0.5f,1.5f));
            }
        }
    }
    IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            float randtime = Random.Range(EnemySpawnTime - EnemySpawnTimeVariance,
                EnemySpawnTime + EnemySpawnTimeVariance);
            yield return new WaitForSecondsRealtime(
                randtime
            );
            while (Time.timeScale < 1)
            {
                yield return new WaitForSecondsRealtime(1);
                continue;
            }
            for (int i = 0; i < NumberOfEnemySpawns; i++)
            {
                int enmIndex = Random.Range(0, Enemies.Count);
                Instantiate(Enemies[enmIndex]);
                yield return new WaitForSecondsRealtime(Random.Range(IndiVidualSpawn.x,IndiVidualSpawn.y));
            }

        }
    }
    IEnumerator MineSpawn()
    {
        while (true)
        {
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            float temptime = Random.Range(MineSpawnTime.x, MineSpawnTime.y);
            yield return new WaitForSecondsRealtime(temptime);
            if (Time.timeScale < 1)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            for (int i = 0; i < NumberOfMineSpawns; i++)
            {
                SpawnMine();
                yield return new WaitForSecondsRealtime(Random.Range(MineIndividualSpawn.x,MineIndividualSpawn.y));
            }
        }
    }
    public void SpawnMine()
    {
        Vector3 playerposition = Player.GetShipPosition();
        Vector3 pos = new Vector3(Random.Range(playerposition.x - Range.x, playerposition.x + Range.x),
                                    Random.Range(playerposition.y - 6, playerposition.y + 6),
                                        Random.Range(playerposition.z + Range.y, playerposition.z + Range.z));
        Instantiate(Mine, pos, Quaternion.identity);
    }
    private int levelCount = 0;
    public int GetLevelCount()
    {
        levelCount++;
        return levelCount;
    }
    public int GetAllowedNumber()
    {
        return allowedNumber;
    }
    public void InstantiateLand()
    {
        // Debug.Log("Instantiating");
        Vector3 dest = new Vector3(-75f, 0, GetLevelCount() * landInstantiateDistanceZ);
        Instantiate(Land, dest, Quaternion.identity);
    }
    public void ReloadLevel(int val = 0)
    {
        SceneManager.LoadScene(val);
    }
    public void Setup()
    {

        EnemySpawnTime -= 0.5f;
        EnemySpawnTimeVariance -= 0.5f;
        MeteorSpawnTime -= 0.3f;
        MeteorSpawnTimeVariance -= 0.3f;
        MineSpawnTime -= new Vector2(0.2f,0.2f);
    }
}
