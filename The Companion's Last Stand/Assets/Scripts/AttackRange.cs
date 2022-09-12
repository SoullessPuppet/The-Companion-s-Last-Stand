using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public List<UnitType> inAttackRangeList;
    UnitType attackRangeOwner;

    private void Start()
    {
        GameObject parentGO = this.transform.parent.gameObject;
        attackRangeOwner = parentGO.GetComponent<UnitType>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<UnitType>() == null)
            return;
        UnitType collidedUnit = collision.gameObject.GetComponent<UnitType>();
        
        if (inAttackRangeList.Contains(collidedUnit))
            return;
        inAttackRangeList.Add(collidedUnit);
        collidedUnit.attackedByList.Add(attackRangeOwner);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<UnitType>() == null)
            return;
        UnitType collidedUnit = collision.gameObject.GetComponent<UnitType>();

        if (!inAttackRangeList.Contains(collidedUnit))
            return;

        inAttackRangeList.Remove(collidedUnit);
        collision.GetComponent<UnitType>().attackedByList.Remove(attackRangeOwner);
    }
}
