using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagGUI : MonoBehaviour {

	public GameObject bagWindow;

	public ItemSlot[] slots;


	public void addItem(int i, Item item)
	{
        slots[i].setIcon(item.GetComponent<SpriteRenderer>().sprite);    
	}

	public void removeItem(int index)
	{
		for(int i = 0; i < slots.Length; i++)
		{
			if(i == index)
			{
                slots[i].setIcon(null);
                return;
			}
		}
	}

    public void clearBag()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].setIcon(null);
        }
    }


    public void openBag()
	{
		bagWindow.SetActive(true);
		WorldManager.instance.slowTime();
	}

	public void closeBag()
	{
		bagWindow.SetActive(false);
		WorldManager.instance.normalTime();
	}

    public void closeBagBtn()
    {
        Player.instance.bag.closeBag();
        Player.instance.closeBagMenu();
    }

}
