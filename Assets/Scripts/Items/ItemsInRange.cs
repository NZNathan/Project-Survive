using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInRange  {

    private List<Item> itemsInRange;
    private Player player;

    public ItemsInRange(Player player)
    {
        this.player = player;
        itemsInRange = new List<Item>();
    }

    public void sortList()
    {
        for (int i = 0; i < itemsInRange.Count; i++)
        {
            for (int j = i + 1; j < itemsInRange.Count; j++)
            {
                Item item1 = itemsInRange[i];
                Item item2 = itemsInRange[j];

                if (Mathf.Abs((item1.transform.position - player.transform.position).magnitude) > Mathf.Abs((item2.transform.position - player.transform.position).magnitude))
                {
                    itemsInRange[i] = item2;
                    itemsInRange[j] = item1;
                }

            }
        }
    }

    public void itemEnterProximity(Item item)
    {
        itemsInRange.Add(item);
        sortList();
        UIManager.instance.newPopup(itemsInRange[0].gameObject);
    }

    public void itemLeaveProximity(Item item)
    {
        UIManager.instance.closePopup();

        if (itemsInRange.Contains(item))
            itemsInRange.Remove(item);
        else
            Debug.LogError("No such item in list: " + item.name);

        if (itemsInRange.Count > 0)
            UIManager.instance.newPopup(itemsInRange[0].gameObject);
    }

    public void pickupItem()
    {
        if (itemsInRange.Count > 0)
        {
            if (player.bag.addItem(itemsInRange[0]))
            {
                Item i = itemsInRange[0];
                i.gameObject.SetActive(false);
            }
        }
    }
}
