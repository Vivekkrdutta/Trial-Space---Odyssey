using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShifter : MonoBehaviour
{
    [SerializeField] int ChangeTime = 15;
    [SerializeField] int CheckTime = 2;
    [HideInInspector]
    public List<Enem_Movement> ActiveEnemies;

    private void Start()
    {
        StartCoroutine(ChangeLeader());
        StartCoroutine(CheckLeader());
    }
    IEnumerator ChangeLeader()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(ChangeTime);
            int count = ActiveEnemies.Count;
            leaderIsThere = false;
            if (count == 0)
                continue;
            foreach (Enem_Movement child in ActiveEnemies)
            {
                child.isAnAttacker = false;
            }
        }
    }
    [HideInInspector]
    public bool leaderIsThere = false;
    IEnumerator CheckLeader()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(CheckTime);
            if (!leaderIsThere)
            {
                if(ActiveEnemies.Count == 0)
                    continue;
                Change();
            }
        }
    }
    public void Change()
    {
        if (ActiveEnemies.Count == 0)
            return;
        int val = Random.Range(0, ActiveEnemies.Count);
        leaderIsThere = true;
        ActiveEnemies[val].isAnAttacker = true;
    }
}
