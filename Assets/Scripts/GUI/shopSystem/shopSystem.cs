using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author Maxy Yuan

public class shopSystem : MonoBehaviour {

	public static shopSystem shopsystem;

	public List<shopItem> shoplist = new List<shopItem>();
	private List<GameObject> itemHolderlist = new List<GameObject> ();

	public List<GameObject> buybutton = new List<GameObject> ();

    public GameObject shopWindow;
    public  GameObject itemHolderPrefab;
    public Text moneyText;
    public Transform grid;

	// Use this for initialization
	public void openShopWindow() {

        shopWindow.SetActive(true);
        shopsystem = this;
		FillList ();
        UpdateUI();
	}

    public void closeShopWindow()
    {
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


			holderScript.itemName.text = shoplist [i].itemName;
			holderScript.itemPrice.text = "$"+ shoplist [i].itemPrice.ToString();
			holderScript.itemID = shoplist [i].itemID;

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
								holderscript.itemPrice.text = "SOLD OUT";
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