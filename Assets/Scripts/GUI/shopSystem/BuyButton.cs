using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour {

	public int itemID;
	public Text buytext; 


	public void Buy(){

		if(itemID ==0 ){

			Debug.Log (" No Item" );

			return;

		}


		for( int i =0;i<shopSystem.shopsystem.shoplist.Count;i++){

			if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought == false && Player.instance.getCoinsAmount() >= shopSystem.shopsystem.shoplist[i].itemPrice){

				shopSystem.shopsystem.shoplist [i].bought = true;
				Player.instance.removeCoins (shopSystem.shopsystem.shoplist [i].itemPrice);
				updateBuyToUse ();
				//WorldManager.instance.currentitemId = itemID;

			}

			else if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought == false && !(Player.instance.getCoinsAmount() >= shopSystem.shopsystem.shoplist[i].itemPrice)){

				Debug.Log ("DONT HAVE ENOUGH MONEY");
			}

			else if(shopSystem.shopsystem.shoplist[i].itemID ==itemID&&shopSystem.shopsystem.shoplist [i].bought){

				// needs to add to bag
				Debug.Log ("HAS BOUGHT ALREADY");
				updateBuyToUse ();
				//WorldManager.instance.currentitemId = itemID;
			}

		}
	
		shopSystem.shopsystem.UpdateSprite (itemID);
	

	}

	void updateBuyToUse(){

	/*	buytext.text = "Using";
		// needs to change weapon on player and update the bag,depending on the ID

		for(int i = 0;i<shopSystem.shopsystem.buybutton.Count;i++){

			BuyButton buyButtonscript = shopSystem.shopsystem.buybutton[i].GetComponent<BuyButton>();


			for(int j = 0;j<shopSystem.shopsystem.shoplist.Count;j++){


				if(shopSystem.shopsystem.shoplist[j].itemID == buyButtonscript.itemID && shopSystem.shopsystem.shoplist [j].bought && shopSystem.shopsystem.shoplist[j].itemID!=itemID){
					

					buyButtonscript.buytext.text = "Use";
				}


			}

		}*/

		//shopSystem.shopsystem.updateBuyButton ();

		shopSystem.shopsystem.UpdateSprite (itemID);

	}

}

