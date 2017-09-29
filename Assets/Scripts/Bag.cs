using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bag
{
    private Item[] items;
    private BagGUI bagGUI;
    private int itemsInBag = 0;
    private bool bagOpen = false;

    public Bag(BagGUI bagGUI)
    {
        items = new Item[9];
        this.bagGUI = bagGUI;

        bagGUI.clearBag();
    }

    public bool hasRoom()
    {
        return itemsInBag < items.Length;
    }

    public bool addItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                bagGUI.addItem(i, item);
                itemsInBag++;
                return true;
            }
        }

        return false;
    }

    public bool addItem(Item item, int i)
    {
        items[i] = item;
        bagGUI.addItem(i, item);
        itemsInBag++;
        return true;
    }

    public int findIndex(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    public void useItem(int i)
    {
        Item it = items[i];
        items[i].useItem();
        
        //If the item hasn't been replace by another item due to the items effect
        if (items[i] == it)
        {
            Debug.Log("Delete Item!");
            items[i] = null;
            itemsInBag--;
        }
        Debug.Log("old Item: " + it + " new Item: " + items[i]);
    }

    public void input()
    {
        if (bagOpen)
            closeBag();
        else
            openBag();
    }

    public void openBag()
    {
        bagGUI.openBag();
        bagOpen = true;
    }

    public void closeBag()
    {
        bagGUI.closeBag();
        bagOpen = false;
    }

    public bool isOpen()
    {
        return bagOpen;
    }

}
