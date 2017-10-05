using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class saveload : MonoBehaviour {
	public GameObject shopWindow;

	// Use this for initialization
	[System.Serializable]
	public class SaveData{

	//public List <shopItem> ShopList = new List<shopItem>();
		public List <Item> ShopList = new List<Item>();


		public float money;
		public int currentItemID;
	}

	public void Saving(){


		SaveData data = new SaveData ();
		//data.money = WorldManager.instance.GetMoneyData ();
		//data.currentItemID = WorldManager.instance.currentitemId;
		for (int i = 0; i < shopSystem.shopsystem.shoplist.Count; i++) {

			data.ShopList.Add (shopSystem.shopsystem.shoplist[i].item);
		}

	/*	BinaryFormatter BF = new BinaryFormatter ();
		FileStream stream = new FileStream (Application.persistentDataPath+"/shop.data",FileMode.Create);

		BF.Serialize (stream,data);
		stream.Close ();*/
	//	print (" shopping saved");
		Debug.Log ("saved");
        shopWindow.SetActive(false);
		


	}

	/*public void loaddata(){

		if(File.Exists(Application.persistentDataPath+"/shop.data")){

			BinaryFormatter BF = new BinaryFormatter ();
			FileStream stream = new FileStream (Application.persistentDataPath+"/shop.data",FileMode.Open);

			SaveData data = (SaveData)BF.Deserialize (stream);
			//WorldManager.instance.SetMoneyData (data.money);
			//WorldManager.instance.currentitemId = data.currentItemID;


			stream.Close ();

			for(int i =0; i<data.ShopList.Count;i++){

				//uodate shop
				shopSystem.shopsystem.shoplist [i] = data.ShopList[i];


				//update sprite
				shopSystem.shopsystem.UpdateSprite(shopSystem.shopsystem.shoplist[i].itemID);

				//update the button
			//	shopSystem.shopsystem.updateBuyButton();

			}

		}


	}*/
}
