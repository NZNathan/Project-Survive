using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAbility : MonoBehaviour, ItemAbility {

    private Equipment equipment;

    private void Start()
    {
        equipment = GetComponent<Equipment > ();
    }

    public void useItem()
	{
        Player.instance.equipItem(equipment);
	}
}
