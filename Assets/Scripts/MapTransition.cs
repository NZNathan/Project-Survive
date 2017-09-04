using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour {

    private bool transitioning = false;

	// Use this for initialization
	void Start ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player" && !transitioning)
        {
            //To avoid the function being called twice set bool to true
            transitioning = true;

            WorldManager.mapLevel++;
            UIManager.instance.newLoadScreen();

            this.enabled = false;
        }
    }

}
