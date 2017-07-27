using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGen : MonoBehaviour {

    Object[] sprites;

	// Use this for initialization
	void Start ()
    {
        sprites = Resources.LoadAll<Sprite>("Characters");

        foreach(Object s in sprites)
        {
            Debug.Log(s);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
