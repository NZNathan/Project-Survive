using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    public Transform cameraRig;
    public Vector3 scrollSpeed;

	
	// Update is called once per frame
	void Update ()
    {
        if (Player.instance != null)
        {
            Player.instance.setInMenu(true);
            Player.instance.gameObject.SetActive(false);
            Player.instance.transform.position += scrollSpeed;
            cameraRig.position += scrollSpeed;
        }
    }
}
