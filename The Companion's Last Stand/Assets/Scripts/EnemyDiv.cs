using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyDiv : UnitType
{
    List<GameObject> possibleTargets = new();
    GameObject curTarget;

    // Start is called before the first frame update
    void Start()
    {
        pather = GetComponent<AIPath>();
        dSetter = GetComponent<AIDestinationSetter>();
        hitbox = GetComponent<CircleCollider2D>();
        selectHitbox = GetComponentInChildren<SelectHitbox>();
        attackRange = GetComponentInChildren<AttackRange>();
        spriteRenderer = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        defaultHitboxRadius = hitbox.radius;
        StartCoroutine("RefreshTargetsRoutine");
        InvokeRepeating("InteractWithinAttackRange", 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Death();
            return; //Skip the rest if hp <= 0:
        }
        ResizeByHP();

        if (locked)
            return; //Skip the rest if locked:
        UpdateMoveSpeed();
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
