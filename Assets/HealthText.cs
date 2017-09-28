using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class HealthText : MonoBehaviour {

	private Text healthText;

	// Use this for initialization
	void Start () 
	{
		healthText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Player.instance != null)
		{
			healthText.text = "HP: " + Player.instance.currentHealth + "/" + Player.instance.maxHealth;
		}
	}
}
