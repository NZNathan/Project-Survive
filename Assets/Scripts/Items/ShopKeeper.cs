using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : Interactable {

	public override void use()
	{
		UIManager.instance.newShopWindow();
	}
}
