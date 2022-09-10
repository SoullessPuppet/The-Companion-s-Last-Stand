using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Structure : UnitType
{
    public int regenPerSec = 1;
    void Start()
    {
        InvokeRepeating("InteractWithinAttackRange", 0, 1);
        InvokeRepeating("StructureRegen", 0, 1);
    }

    void Update()
    {
        if (hp <= 0)
        {
            hp = 1;
            StructureCapture();
        }
    }

    void StructureCapture()
    {
        bool allyCapture = true;
        bool enemyCapture = true;
        foreach (GameObject go in attackedByList)
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
