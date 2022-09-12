using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AllyDiv : UnitType
{
    void Start()
    {
        pather = GetComponent<AIPath>();
        dSetter = GetComponent<AIDestinationSetter>();
        hitbox = GetComponent<CircleCollider2D>();
        selectHitbox = GetComponentInChildren<SelectHitbox>();
        attackRange = GetComponentInChildren<AttackRange>();
        spriteRenderer = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        defaultHitboxRadius = hitbox.radius;
        InvokeRepeating("InteractWithinAttackRange", 0, 0.1f);
    }

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
}
