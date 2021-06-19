using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatAttacks
{
    public string AttackName;
    public string nextLightAttack;
    public string nextHeavyAttack;
    public string nextDownAttack;

    public bool isUnlocked;
    public bool endOfAttackString;
    public bool willMoveHor, willMoveVert;
    public float attackDur;

    public bool canMoveWhileAttack;
    public Vector2 movementChange;
}
