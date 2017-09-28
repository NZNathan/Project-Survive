using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality { ITEM, COMMON, RARE, EPIC, LENGENDARY }

public class Equipment : Item {

	//Stat Mods
	public int strMod = 0;
	public int aglMod = 0;
	public int endMod = 0;

    protected void Start()
    {
        itemAbility = GetComponent<EquipmentAbility>();
    }

}
