using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogBG : MonoBehaviour {

	public SpriteRenderer[] fogs;

	public Vector3 fogMovement = new Vector3(0.05f, 0, 0);

	//Fade effects
	public float fadeStep = 0.05f;
	public float fadeLowest = 220;
	public int fadeHighest = 255;
	
	// Update is called once per frame
	void Update () 
	{
		foreach(SpriteRenderer fog in fogs)
		{
			fog.transform.position += fogMovement;
		}
	}
}
