using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag {

	private Item[] items;
	private BagGUI bagGUI;

	private bool bagOpen = false;

	public Bag(BagGUI bagGUI)
	{
		items = new Item[9];
		this.bagGUI = bagGUI;
	}

	public void input()
	{
		if(bagOpen)
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
