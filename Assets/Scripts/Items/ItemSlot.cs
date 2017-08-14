using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour {

    Image itemIcon;

    void Start()
    {
        itemIcon = GetComponent<Image>();
    }

    public void useItem(int index)
    {
        if (itemIcon.sprite != null)
        {
            Player.instance.useItem(index);
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }
    }

	public void setIcon(Sprite sprite)
    {
        itemIcon = GetComponent<Image>();
        itemIcon.sprite = sprite;

        if (sprite != null)
            itemIcon.enabled = true;
        else
            itemIcon.enabled = false;
    }

}
