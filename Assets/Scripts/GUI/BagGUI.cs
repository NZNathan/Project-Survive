using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagGUI : MonoBehaviour {

	public GameObject bagWindow;
    public Text coinText;
	private ItemSlot[] slots;

    private void Start()
    {
        slots = GetComponentsInChildren<ItemSlot>();
        bagWindow.SetActive(false);
    }

    public void addItem(int i, Item item)
	{
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
        coinText.text = "Coins: " + Player.instance.getCoinsAmount();
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
