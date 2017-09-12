using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author Maxy Yuan

public class shopSystem : MonoBehaviour {

	public static shopSystem shopsystem;

	public List<shopItem> shoplist = new List<shopItem>();
	private List<GameObject> itemHolderlist = new List<GameObject> ();

    public GameObject shopWindow;
	public  GameObject itemHolderPrefab;
    public Text moneyText;
	public Transform grid;


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
        moneyText.text = "$ " + Player.instance.getGoldAmount().ToString("N1");
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


			if (shoplist [i].bought) {
				holderScript.itemImage.sprite = shoplist [i].boughtSprite;
			}

			else {
				holderScript.itemImage.sprite = shoplist [i].unboughtSprite;

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
							


						}


					}


				}
			}



		}


	}


}
