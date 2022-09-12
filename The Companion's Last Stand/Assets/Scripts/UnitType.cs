using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitType : MonoBehaviour
{
    [HideInInspector] public SelectHitbox selectHitbox;
    [HideInInspector] public AttackRange attackRange;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public float hp = 100;
    public float maxHP = 100;
    public float attackSlow = 0.75f;
    public float damageResist = 0;
    public float attackPower = 0;
    public float healPower = 0;
    public float defaultMoveSpeed = 1;
    public bool locked = false;
    [HideInInspector] public int team;
    [HideInInspector] public UnitType closestTarget;
    [HideInInspector] public float speedBonus = 0;
    [HideInInspector] public List<UnitType> attackedByList;
    [HideInInspector] public CircleCollider2D hitbox;
    [HideInInspector] public float defaultHitboxRadius;
    [HideInInspector] public static float minSize = 0.5f;
    [HideInInspector] public AIPath pather;
    [HideInInspector] public AIDestinationSetter dSetter;

    public void Attack(UnitType attackTarget, float power)
    {
        float damageToDeal = power * (1 - attackTarget.damageResist);
        attackTarget.hp -= damageToDeal;
    }

    public void Heal(UnitType healTarget, float power)
    {
        healTarget.hp += power;
        if (healTarget.hp > healTarget.maxHP)
            healTarget.hp = healTarget.maxHP;
    }

    public void AttackClosestUnit()
    {
        closestTarget = null; //Reset closest target
        List<UnitType> withinRangeList = attackRange.inAttackRangeList;
        if (withinRangeList.Count == 0)
            return; //Skip if no unit is in range
        //Find closest target
        float closestDist = Mathf.Infinity;
        foreach (UnitType unit in withinRangeList)
        {
            if (team != unit.team)
            {
                float dist = Vector2.Distance(unit.transform.position, transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestTarget = unit;
                }
            }
        }
        if (closestTarget == null)
            HealClosestUnit(); //If no target to attack, check if any target to heal
        else if (closestTarget != null)
            Attack(closestTarget, attackPower);
    }

    public void HealClosestUnit()
    {
        closestTarget = null; //Reset closest target
        List<UnitType> withinRangeList = attackRange.inAttackRangeList;
        if (withinRangeList.Count == 0)
            return; //Skip if no unit is in range
        //Find closest target
        float closestDist = Mathf.Infinity;
        foreach (UnitType unit in withinRangeList)
        {
            if (team == unit.team)
            {
                float dist = Vector2.Distance(unit.transform.position, transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestTarget = unit;
                }
            }
        }
        if (closestTarget != null)
            Heal(closestTarget, healPower);
    }

    public void ResizeByHP()
    {
        float percentHPLost = 1 - (hp / maxHP);
        float newSize = 1 - (percentHPLost * minSize);
        hitbox.radius = defaultHitboxRadius * newSize;
        spriteRenderer.transform.localScale = new Vector3(newSize, newSize, 1);
    }

    public void UpdateMoveSpeed()
    {
        float currentSlow = 0;
        if (attackedByList != null)
        {
            foreach (UnitType unit in attackedByList)
            {
                float slow = 0;
                if (unit.GetComponent<EnemyDiv>() != null)
                    slow = unit.GetComponent<EnemyDiv>().attackSlow;
                else if (unit.GetComponent<PlayerDiv>() != null)
                    slow = unit.GetComponent<PlayerDiv>().attackSlow;
                else if (unit.GetComponent<AllyDiv>() != null)
                    slow = unit.GetComponent<AllyDiv>().attackSlow;

                if (slow > currentSlow)
                {
                    currentSlow = slow;
                }
            }
        }
        pather.maxSpeed = defaultMoveSpeed * (1 - currentSlow + speedBonus);
    }

    public void InteractWithinAttackRange()
    {
        //Attack (Higher priority) or Heal
        if (attackPower > 0)
            AttackClosestUnit();
        else if (healPower > 0)
            HealClosestUnit();
    }

    public virtual void Death()
    {

    }
}
