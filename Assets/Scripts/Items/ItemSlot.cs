using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Item Descriptors
    private string itemName;
    private string itemDescription;
    private int itemPrice;

    //Item Sprite
    public Image itemIcon;

    private void Start()
    {
        itemIcon = GetComponentsInChildren<Image>()[1];
    }

    public void useItem(int index)
    {
        if (itemIcon.sprite != null)
        {
            Player.instance.useItem(index);
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            UIManager.instance.closeTooltip();
        }
    }

	public void setIcon(Sprite sprite, Item item)
    {
        itemIcon.sprite = sprite;

        if (sprite != null)
        {
            itemIcon.enabled = true;
            itemName = item.itemName;
            itemDescription = item.itemDescription;
            itemPrice = item.itemPrice;
        }
        else
            itemIcon.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemIcon.enabled == true)
            UIManager.instance.newTooltip(itemName, itemDescription, itemPrice);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemIcon.enabled == true)
            UIManager.instance.closeTooltip();
    }

}
