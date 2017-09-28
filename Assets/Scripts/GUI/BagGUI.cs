using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagGUI : MonoBehaviour {

	public GameObject bagWindow;

	private ItemSlot[] slots;

    private void Start()
    {
        slots = GetComponentsInChildren<ItemSlot>();
        bagWindow.SetActive(false);
    }

    public void addItem(int i, Item item)
	{
        Debug.Log(item + " sprite: " + item.GetComponent<SpriteRenderer>().sprite);
        slots[i].setIcon(item.GetComponent<SpriteRenderer>().sprite, item);

	}

	public void removeItem(int index)
	{
		for(int i = 0; i < slots.Length; i++)
		{
			if(i == index)
			{
                slots[i].setIcon(null, null);
                return;
			}
		}
	}

    public void clearBag()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].setIcon(null, null);
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
        UIManager.instance.closeTooltip();
		WorldManager.instance.normalTime();
	}

    public void closeBagBtn()
    {
        Player.instance.bag.closeBag();
        Player.instance.closeBagMenu();
    }

}
