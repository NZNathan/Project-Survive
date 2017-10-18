using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour {

	public int itemID;
	public Text buytext; 
	public bool addtoBag;


	public void Buy(){

	

		if(itemID < 0 ){

			Debug.Log (" No Item" );

			return;

		}


		for( int i =0;i<shopSystem.shopsystem.shoplist.Count;i++){

			if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought == false && Player.instance.getCoinsAmount() >= shopSystem.shopsystem.shoplist[i].item.itemPrice){

				shopSystem.shopsystem.shoplist [i].bought = true;
				Player.instance.removeCoins (shopSystem.shopsystem.shoplist [i].item.itemPrice);
				//updateBuyToUse ();
				//WorldManager.instance.currentitemId = itemID;

			if(Player.instance.bag.hasRoom()){

					// needs to add to bag
					Item item = (Item) Instantiate(shopSystem.shopsystem.shoplist [i].item);
					item.gameObject.SetActive(false);
					item.Start();
					Player.instance.bag.addItem(item);
                    shopSystem.shopsystem.UpdateUI();

					updateBuyToUse ();
					//WorldManager.instance.currentitemId = itemID;
				}

			}
				


			else if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought == false && !(Player.instance.getCoinsAmount() >= shopSystem.shopsystem.shoplist[i].item.itemPrice)){

				Debug.Log ("DONT HAVE ENOUGH MONEY");
			}


	
		shopSystem.shopsystem.UpdateSprite (itemID);
	

	}
	}

	void updateBuyToUse(){

		shopSystem.shopsystem.UpdateSprite (itemID);

	}

}

