using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author Maxy Yuan

public class shopSystem : MonoBehaviour {

	public static shopSystem shopsystem;

	public List<shopItem> shoplist = new List<shopItem>();
	//public List<Item> shoplist = new List<Item>();
	private List<GameObject> itemHolderlist = new List<GameObject> ();


	public List<GameObject> buybutton = new List<GameObject> ();

    public GameObject shopWindow;
    public  GameObject itemHolderPrefab;
    public Text moneyText;
    public Transform grid;
	public bool ShopActive = false;
	public bool FirstOpenShop = true;

	// Use this for initialization
	public void openShopWindow()
    {

        Player.instance.setInMenu(true);

		if( FirstOpenShop==true ){
			if (ShopActive == false) {
				shopWindow.SetActive (true);
				shopsystem = this;
				FillList ();
				UpdateUI ();
				ShopActive = true;
				FirstOpenShop = false;

			} 

			else {
				ShopActive = false;
				FirstOpenShop= false;

			}


		}

		if(FirstOpenShop == false){
			if(ShopActive == false){
				
				shopWindow.SetActive (true);

			}
			ShopActive = false;


		}

	}

    public void closeShopWindow()
    {
        Player.instance.setInMenu(false);
        shopWindow.SetActive(false);
    }

    public void UpdateUI()
    {
        moneyText.text = "Coins: " + Player.instance.getCoinsAmount();
    }

    void FillList(){

		for (int i = 0; i < shoplist.Count; i++) {
			//Debug.Log ("run " + i);
			GameObject holder= Instantiate (itemHolderPrefab,grid,false);
			itemHolder holderScript = holder.GetComponent<itemHolder> ();


			//holderScript.item.itemName = shoplist [i].item.itemName;
			//holderScript.item.itemPrice = shoplist [i].item.itemPrice;
			//holderScript.itemName = shoplist [i].itemName;
			//holderScript.itemPrice = shoplist [i].itemPrice;
			holderScript.itemID = shoplist [i].itemID;
			holderScript.item = shoplist[i].item;
			holderScript.itemName.text = shoplist [i].item.itemName;
		//	holderScript.itemPrice.text = shoplist [i].item.itemPrice;

			//buy button
			holderScript.buyButton.GetComponent<BuyButton>().itemID =shoplist [i].itemID ;

			//handle list
			itemHolderlist.Add(holder);
			buybutton.Add (holderScript.buyButton);


			if (shoplist [i].bought) {
				holderScript.itemImage.sprite = Resources.Load<Sprite>("sprite/"+ shoplist [i].boughtSpriteName);

			}

			else {
				holderScript.itemImage.sprite = Resources.Load<Sprite>("sprite/"+ shoplist [i].unboughtSpriteName);

			}			//older version: holder.transform.SetParent (grid);

		}


	}
	public void UpdateSprite(int itemID){
		for(int i = 0;i<itemHolderlist.Count;i++){

			itemHolder holderscript = itemHolderlist [i].GetComponent<itemHolder> ();
			if( holderscript.itemID == itemID){

				for( int j = 0;j<shoplist.Count;j++){
					if(shoplist[j].itemID == itemID){

						if(shoplist[j].bought){


							if (shoplist [j].bought) {
								holderscript.itemImage.sprite = Resources.Load<Sprite>("sprite/"+ shoplist [j].boughtSpriteName);
								//holderscript.item.itemName = "SOLD OUT";
							}

							else {
								holderscript.itemImage.sprite = Resources.Load<Sprite>("sprite/"+ shoplist [j].unboughtSpriteName);

							}



						}


					}


				}
			}



		}


	}

	/*public void updateBuyButton(){

		int currentitemID = WorldManager.instance.currentitemId;

		for(int i = 0;i <buybutton.Count;i++){
			BuyButton buybuttonscript = buybutton [i].GetComponent<BuyButton> ();
			for(int j = 0; j< shoplist.Count;i++){

				if(shoplist[j].itemID == buybuttonscript.itemID&&shoplist[j].bought&& shoplist[j].itemID!=currentitemID){

					buybuttonscript.buytext.text = "Use";

				}

				else if(shoplist[j].itemID == buybuttonscript.itemID&&shoplist[j].bought&& shoplist[j].itemID==currentitemID){
					
					buybuttonscript.buytext.text = "Using";

				}

			}


		}


	}*/

}