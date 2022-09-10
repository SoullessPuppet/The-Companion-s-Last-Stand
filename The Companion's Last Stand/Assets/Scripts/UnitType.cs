using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UnitType : MonoBehaviour
{
    public SelectHitbox selectHitbox;
    public CircleCollider2D attackRange;
    public SpriteRenderer spriteRenderer;
    public float hp = 100;
    public float maxHP = 100;
    public float attackSlow = 0.75f;
    public float damageResist = 0;
    public float attackPower = 0;
    public float healPower = 0;
    public float defaultMoveSpeed = 1;
    public bool locked = false;
    [HideInInspector] public float speedBonus = 0;
    [HideInInspector] public List<GameObject> attackedByList;
    [HideInInspector] public CircleCollider2D hitbox;
    [HideInInspector] public float defaultHitboxRadius;
    [HideInInspector] public AIPath pather;
    [HideInInspector] public AIDestinationSetter dSetter;

    public void Attack(GameObject go, float power)
    {
        UnitType attackTarget = null;
        if (go.GetComponent<EnemyDiv>() != null)
        {
            attackTarget = go.GetComponent<EnemyDiv>();
        }
        else if (go.GetComponent<PlayerDiv>() != null)
        {
            attackTarget = go.GetComponent<PlayerDiv>();
        }
        else if (go.GetComponent<AllyDiv>() != null)
        {
            attackTarget = go.GetComponent<AllyDiv>();
        }
        else if (go.GetComponent<Structure>() != null)
        {
            attackTarget = go.GetComponent<Structure>();
        }

        if (attackTarget != null)
        {
            float damageToDeal = power * (1 - attackTarget.damageResist);
            attackTarget.hp -= damageToDeal;
        }
    }

    public void Heal(GameObject go, float power)
    {
        UnitType healTarget = null;
        if (go.GetComponent<EnemyDiv>() != null)
        {
            healTarget = go.GetComponent<EnemyDiv>();
        }
        else if (go.GetComponent<PlayerDiv>() != null)
        {
            healTarget = go.GetComponent<PlayerDiv>();
        }
        else if (go.GetComponent<AllyDiv>() != null)
        {
            healTarget = go.GetComponent<AllyDiv>();
        }
        //No normal healing for Structures
        if (healTarget != null)
        {
            healTarget.hp += power;
            if (healTarget.hp > healTarget.maxHP)
                healTarget.hp = healTarget.maxHP;
        }
    }

    public void InteractWithinAttackRange()
    {
        List<GameObject> withinRangeList = attackRange.GetComponent<AttackRange>().inAttackRangeList;
        foreach (GameObject interactTarget in withinRangeList)
        {
            if (gameObject.CompareTag("EnemyTeam"))
            {
                if (interactTarget.CompareTag("EnemyTeam"))
                    Heal(interactTarget, healPower);
                else if (interactTarget.CompareTag("PlayerTeam") || interactTarget.CompareTag("AllyTeam"))
                    Attack(interactTarget, attackPower);
            }
            else if (gameObject.CompareTag("PlayerTeam") || gameObject.CompareTag("AllyTeam"))
            {
                if (interactTarget.CompareTag("EnemyTeam"))
                    Attack(interactTarget, attackPower);
                else if (interactTarget.CompareTag("PlayerTeam") || interactTarget.CompareTag("AllyTeam"))
                    Heal(interactTarget, healPower);
            }
        }
        //Need rework. Unit attacks closest enemy target in range, and if none available, heal closest ally target in range.
    }

    public void ResizeByHP()
    {
        float percentHPLeft = hp / maxHP;
        hitbox.radius = defaultHitboxRadius * percentHPLeft;
        spriteRenderer.transform.localScale = new Vector3(percentHPLeft, percentHPLeft, 1);
    }

    public void UpdateMoveSpeed()
    {
        float currentSlow = 0;
        if (attackedByList != null)
        {
            foreach (GameObject go in attackedByList)
            {
                float slow = 0;
                if (go.GetComponent<EnemyDiv>() != null)
                    slow = go.GetComponent<EnemyDiv>().attackSlow;
                else if (go.GetComponent<PlayerDiv>() != null)
                    slow = go.GetComponent<PlayerDiv>().attackSlow;
                else if (go.GetComponent<AllyDiv>() != null)
                    slow = go.GetComponent<AllyDiv>().attackSlow;

                if (slow > currentSlow)
                {
                    currentSlow = slow;
                }
            }
        }
        pather.maxSpeed = defaultMoveSpeed * (1 - currentSlow + speedBonus);
    }

    public void DeathEffects()
    {

    }
}
