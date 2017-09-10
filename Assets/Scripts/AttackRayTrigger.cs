using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRayTrigger : MonoBehaviour {

    private int comboNumber = 0;
    private bool attackTriggered = false;
    private bool attackIsOver = false;
    private CMoveCombatable character;

    private void Start()
    {
        character = GetComponentInParent<CMoveCombatable>();
    }

    public void attackTrigger()
    {
        attackTriggered = true;
        Invoke("resetTrigger", 1f);
    }

    public bool hasAttackTriggered()
    {
        return attackTriggered;
    }

    public void resetTrigger()
    {
        attackTriggered = false;
    }

    public void attackOver()
    {
        attackIsOver = true;
        Invoke("resetAttack", 1f);
    }

    public bool isAttackOver()
    {
        return attackIsOver;
    }

    public void resetAttack()
    {
        attackIsOver = false;
    }

    public void idleState()
    {
        character.canCombo = false;
    }
}
