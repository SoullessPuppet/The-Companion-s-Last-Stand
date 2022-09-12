using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Structure : UnitType
{
    public int regenPerSec = 1;
    void Start()
    {
        hitbox = GetComponent<CircleCollider2D>();
        selectHitbox = GetComponentInChildren<SelectHitbox>();
        attackRange = GetComponentInChildren<AttackRange>();
        spriteRenderer = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        if (CompareTag("PlayerTeam") || CompareTag("AllyTeam"))
            team = 1;
        else if (CompareTag("EnemyTeam"))
            team = -1;

        InvokeRepeating("InteractWithinAttackRange", 0, 0.1f);
        InvokeRepeating("StructureRegen", 0, 0.1f);
    }

    void Update()
    {
        if (hp <= 0)
        {
            hp = 1;
            Death();
        }
    }

    public override void Death()
    {
        bool allyCapture = true;
        bool enemyCapture = true;
        foreach (UnitType go in attackedByList)
        {
            if (go.CompareTag("AllyTeam") || go.CompareTag("PlayerTeam"))
                enemyCapture = false;
            else if (go.CompareTag("EnemyTeam"))
                allyCapture = false;
        }
        if (allyCapture && !enemyCapture)
        {
            gameObject.tag = "AllyTeam";
            //sprite switch
        }
        else if (enemyCapture && !allyCapture)
        {
            gameObject.tag = "EnemyTeam";
            //sprite switch
        }
    }

    void StructureRegen()
    {
        hp += regenPerSec;
    }

    void HealStructure(float power)
    {
        hp += power;
        if (hp > maxHP)
            hp = maxHP;
    }
}
