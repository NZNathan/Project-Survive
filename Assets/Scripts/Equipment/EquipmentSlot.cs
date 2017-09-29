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
            if (Player.instance.bag.hasRoom())
            {
                Equipment eq = Player.instance.unequipItem(i);
                Player.instance.bag.addItem(eq);
                itemName = "";
                itemIcon.sprite = defualtIcon;
                UIManager.instance.closeTooltip();
            }
        }
    }

}
