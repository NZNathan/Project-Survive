using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	private bool withinRange = false;

	public void use()
	{
        WorldManager.instance.increaseLevel();
        UIManager.instance.newLoadScreen();

        this.enabled = false;
	}


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
			withinRange = true;
            UIManager.instance.newPopup(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
			withinRange = false;
            UIManager.instance.closePopup();
        }
    }

	void Update()
	{
		bool eKeyDown = Input.GetKeyDown(KeyCode.E);

		if(eKeyDown && withinRange)
			use();
	}
}
