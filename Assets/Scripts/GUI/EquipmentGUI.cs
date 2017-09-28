using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGUI : MonoBehaviour {

    public EquipmentSlot[] slots;
	
	public void equipItem(int i, Equipment eq)
    {
        slots[i].setIcon(eq.GetComponent<SpriteRenderer>().sprite, eq);
    }

}
