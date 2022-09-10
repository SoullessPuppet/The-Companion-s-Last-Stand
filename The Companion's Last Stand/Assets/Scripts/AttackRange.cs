using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public List<GameObject> inAttackRangeList;
    GameObject attackRangeOwner;

    private void Start()
    {
        attackRangeOwner = this.transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inAttackRangeList.Contains(collision.gameObject))
            return;

        inAttackRangeList.Add(collision.gameObject);
        if (collision.CompareTag("PlayerTeam"))
        {
            collision.GetComponent<PlayerDiv>().attackedByList.Add(attackRangeOwner);
        }
        else if (collision.CompareTag("EnemyTeam"))
        {
            collision.GetComponent<EnemyDiv>().attackedByList.Add(attackRangeOwner);
        }
        else if (collision.CompareTag("AllyTeam"))
        {
            collision.GetComponent<AllyDiv>().attackedByList.Add(attackRangeOwner);
        }
        //AllyDiv too
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!inAttackRangeList.Contains(collision.gameObject))
            return;

        inAttackRangeList.Remove(collision.gameObject);
        if (collision.CompareTag("PlayerTeam"))
        {
            collision.GetComponent<PlayerDiv>().attackedByList.Remove(attackRangeOwner);
        }
        else if (collision.CompareTag("EnemyTeam"))
        {
            collision.GetComponent<EnemyDiv>().attackedByList.Remove(attackRangeOwner);
        }
        else if (collision.CompareTag("AllyTeam"))
        {
            collision.GetComponent<AllyDiv>().attackedByList.Remove(attackRangeOwner);
        }
    }
}
