using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyDiv : MonoBehaviour
{
    public SelectHitbox selectHitbox;
    public CircleCollider2D attackRange;
    AIPath pather;
    AIDestinationSetter dSetter;
    public float attackPower = 0;
    public float attackSlow = 0.5f;
    public float damageResist = 0;
    public float moveSpeed = 1;
    public bool locked = false;
    List<GameObject> possibleTargets = new();
    GameObject curTarget;

    // Start is called before the first frame update
    void Start()
    {
        pather = GetComponent<AIPath>();
        dSetter = GetComponent<AIDestinationSetter>();
        StartCoroutine("RefreshTargetsRoutine");
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
            return;
        //Only when not locked:
        pather.maxSpeed = moveSpeed;
    }

    IEnumerator RefreshTargetsRoutine()
    {
        //Refresh possible targets list
        GameObject[] playerTeam = GameObject.FindGameObjectsWithTag("PlayerTeam");
        GameObject[] allyTeam = GameObject.FindGameObjectsWithTag("AllyTeam");

        foreach (GameObject go in playerTeam)
        {
            possibleTargets.Add(go);
        }
        foreach (GameObject go in allyTeam)
        {
            possibleTargets.Add(go);
        }

        //Switch to closest target
        float closestDist = Mathf.Infinity;
        foreach (GameObject go in possibleTargets)
        {
            float dist = Vector2.Distance(go.transform.position, transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                curTarget = go;
            }
        }
        dSetter.target = curTarget.transform;

        yield return new WaitForSeconds(1);
        StartCoroutine("RefreshTargetsRoutine");
    }
}
