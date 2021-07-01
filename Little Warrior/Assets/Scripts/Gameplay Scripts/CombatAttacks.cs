using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum knockbackDir
{
    Vertical,
    Horizontal,
    Diagonal
}

[System.Serializable]
public class CombatAttacks
{
    [Header("Attack Name Details (Name of Attack, next Attacks)")]
    public string AttackName;
    public string nextLightAttack;
    public string nextHeavyAttack;
    public string previousAttackName;
    public string nextDownAttack;

    [Header("Boolean Conditions")]
    public bool isUnlocked;
    public bool endOfAttackString;
    public bool willMoveHor, willMoveVert;

    [Header("Duration of attack, damage values")]
    public float attackDur;
    public float damage;

    [Header("Movement Info")]
    public bool canMoveWhileAttack;
    public Vector2 movementChange;

    [Header("Knockback Details")]
    public knockbackInfo knockback;
}

[System.Serializable]
public class knockbackInfo
{
    public bool willKnockback;
    public Vector2 knockbackForce;
    public float knockbackDur;
    public knockbackDir knockbackDirection;
}
