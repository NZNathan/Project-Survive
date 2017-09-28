using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Item Descriptors
    protected string itemName = "";
    protected string itemDescription = "";
    protected int itemPrice = 0;
    protected Quality quality;

    //Item Sprite
    [HideInInspector]
    public Image itemIcon;

    protected void Start()
    {
        itemIcon = GetComponentsInChildren<Image>()[1];
    }

    public virtual void useItem(int index)
    {
        if (itemIcon.sprite != null)
        {
            Player.instance.useItem(index);
            itemIcon.sprite = null;
            itemName = "";
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
            quality = item.quality;
        }
        else
            itemIcon.enabled = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(itemIcon.enabled == true && itemName != "")
            UIManager.instance.newTooltip(itemName, itemDescription, itemPrice, quality.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemIcon.enabled == true)
            UIManager.instance.closeTooltip();
    }

}
