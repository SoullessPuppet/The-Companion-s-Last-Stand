using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerDiv : UnitType
{
    public GameObject gatherPoint;

    // Start is called before the first frame update
    void Start()
    {
        pather = GetComponent<AIPath>();
        hitbox = GetComponent<CircleCollider2D>();
        defaultHitboxRadius = hitbox.radius;
        InvokeRepeating("InteractWithinAttackRange", 0, 1);
    }

    // Update is called once per frame
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

        if (!selectHitbox.selected)
            return; //Skip the rest if not selected:
        //Right-click to set gather point
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gatherPoint.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
}
