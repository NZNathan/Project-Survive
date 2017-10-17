using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class itemHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	public Text itemName;
	public Text itemPrice;
	public Image itemImage;


	public Item item;

	public int itemID;
	public GameObject buyButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        shopSystem.shopsystem.descriptionText.text = item.itemDescription;
        shopSystem.shopsystem.itemTitle.text = item.itemName; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shopSystem.shopsystem.descriptionText.text = "";
        shopSystem.shopsystem.itemTitle.text = "";
    }
}
