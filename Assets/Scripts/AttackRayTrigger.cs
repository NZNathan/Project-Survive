using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRayTrigger : MonoBehaviour {

    private int comboNumber = 0;
    private bool attackTriggered = false;
    private CMoveCombatable character;

    private void Start()
    {
        character = GetComponentInParent<CMoveCombatable>();
    }

    public void attackTrigger()
    {
        attackTriggered = true;
    }

    public bool hasAttackTriggered()
    {
        return attackTriggered;
    }

    public void resetTrigger()
    {
        attackTriggered = false;
    }

    public void idleState()
    {
        character.canCombo = false;
    }
}
