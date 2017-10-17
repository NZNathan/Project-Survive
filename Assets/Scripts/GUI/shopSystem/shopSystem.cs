using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author Maxy Yuan

public class shopSystem : MonoBehaviour {

	public static shopSystem shopsystem;
	public string temp;

	public List<shopItem> shoplist = new List<shopItem>();

	private List<GameObject> itemHolderlist = new List<GameObject> ();


	public List<GameObject> buybutton = new List<GameObject> ();

    public GameObject shopWindow;
    public  GameObject itemHolderPrefab;
    public Text moneyText;
    public Text itemTitle;
    public Text descriptionText;
    public Transform grid;
	public bool ShopActive = false;
	public bool FirstOpenShop = true;

	// Use this for initialization
	public void openShopWindow() {

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
        shopWindow.SetActive(false);
        Player.instance.setInMenu(false);
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
		
			//pass the item Id
			holderScript.itemID = shoplist [i].itemID;

			// pass the item
			holderScript.item = shoplist[i].item;

			//pass the name
			holderScript.itemName.text = shoplist [i].item.itemName;

			// pass the price
			holderScript.itemPrice.text = holderScript.item.itemPrice.ToString();


			//buy button
			holderScript.buyButton.GetComponent<BuyButton>().itemID =shoplist [i].itemID ;

			//handle list
			itemHolderlist.Add(holder);
			buybutton.Add (holderScript.buyButton);


			if (shoplist [i].bought) {

                Color c = holderScript.itemImage.color;

                c.r = 0.5f;
                c.g = 0.5f;
                c.b = 0.5f;

                holderScript.itemImage.color = c;

			}

			else {
				holderScript.itemImage.sprite = shoplist[i].sprite;

			}		

		}


	}
	public void UpdateSprite(int itemID){
		for(int i = 0;i<itemHolderlist.Count;i++){

			itemHolder holderScript = itemHolderlist [i].GetComponent<itemHolder> ();
			if(holderScript.itemID == itemID){

				for( int j = 0;j<shoplist.Count;j++){
					if(shoplist[j].itemID == itemID){

						if(shoplist[j].bought){


							if (shoplist [j].bought) {

                                Color c = holderScript.itemImage.color;

                                c.r = 0.5f;
                                c.g = 0.5f;
                                c.b = 0.5f;

                                holderScript.itemImage.color = c;

                            }

							else {
                                holderScript.itemImage.sprite = shoplist[i].sprite;

                            }



						}


					}


				}
			}



		}


	}


}