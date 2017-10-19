using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour {

    public bool tutorial = false;
	protected bool withinRange = false;


	public virtual void use()
	{
        if (tutorial)
        {
            SceneManager.LoadSceneAsync("Main");
        }
        else
        {
            MusicManager.instance.resetField();
            WorldManager.instance.increaseLevel();
            UIManager.instance.newLoadScreen();
        }

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
		bool eKeyDown = Input.GetKeyUp(KeyCode.E);

		if(eKeyDown && withinRange)
			use();
	}
}
