using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerDiv : MonoBehaviour
{
    public GameObject target;
    public SelectHitbox selectHitbox;
    public CircleCollider2D attackRange;
    AIPath pather;
    public float attackPower = 0;
    public float attackSlow = 0.5f;
    public float damageResist = 0;
    public float moveSpeed = 1;
    public bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        pather = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
            return;
        //Only when not locked:
        pather.maxSpeed = moveSpeed;

        if (!selectHitbox.selected)
            return;
        //Only when selected:
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
}
