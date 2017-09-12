using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Maxy Yuan

public class BuyButton : MonoBehaviour {

	public int itemID;

	public void Buy(){

		if(itemID ==0 ){
			
			Debug.Log (" No Item" );

			return;

		}


		for( int i = 0; i < shopSystem.shopsystem.shoplist.Count; i++){

			if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought == false && Player.instance.getGoldAmount() >= shopSystem.shopsystem.shoplist[i].itemPrice){

				shopSystem.shopsystem.shoplist [i].bought = true;
				Player.instance.removeGold(shopSystem.shopsystem.shoplist[i].itemPrice);
                shopSystem.shopsystem.UpdateUI();

            }
		}

	}


}
