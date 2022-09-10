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
        defaultHitboxRadius = hitbox.radius;
    }

    void Update()
    {
        if (hp <= 0)
        {
            DeathEffects();
            return; //Skip the rest if hp <= 0:
        }
        ResizeByHP();

        if (locked)
            return; //Skip the rest if locked:
        UpdateMoveSpeed();
    }
}
