using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, ItemAbility
{ 
    public int healAmount = 0;

    public void useItem()
    {
        Player.instance.recoverHealth(healAmount);
    }

}
