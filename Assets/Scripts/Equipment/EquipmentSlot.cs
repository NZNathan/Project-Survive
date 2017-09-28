using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EquipmentSlot : ItemSlot {

    public Sprite defualtIcon;

    private new void Start()
    {
        base.Start();
    }

    public void unequipItem(int i)
    {
        if (itemIcon.sprite != defualtIcon)
        {
            if (Player.instance.unequipItem(i))
            {
                itemName = "";
                itemIcon.sprite = defualtIcon;
                UIManager.instance.closeTooltip();
            }
        }
    }

}
