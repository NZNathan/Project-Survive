using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagGUI : MonoBehaviour {

	public GameObject bagWindow;

	public Image[] slots;

	// Use this for initialization
	void Start () 
	{
		
	}

	public void addItem(int i, Item item)
	{
        slots[i].sprite = item.GetComponent<SpriteRenderer>().sprite;
        slots[i].enabled = true;
	}

	public void removeItem(int index)
	{
		for(int i = 0; i < slots.Length; i++)
		{
			if(i == index)
			{
				slots[i].sprite = null;
                slots[i].enabled = false;
                return;
			}
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
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
