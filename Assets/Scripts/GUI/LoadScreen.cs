using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreen : MonoBehaviour {

    public CanvasGroup loadScreen;
    public Text levelText;
    public Text loadText;

    //Animation Variables
    private int count = 0;
    private float fadeStep = 0.05f;

    private void Start()
    {
        activate();
    }

    public void activate()
    {
        StartCoroutine("transitionToNewMap");
    }

    void loadTextAnimation()
    {
        if(count == 3)
        {
            loadText.text = "Loading";
            count = 0;
        }
        else
        {
            loadText.text += ".";
            count++;
        }
    }

    IEnumerator transitionToNewMap()
    {
        Player.instance.setInMenu(true);

        loadScreen.gameObject.SetActive(true);
        levelText.text = "Forest 1-" + WorldManager.mapLevel;

        //Fade in screen
        while (loadScreen.alpha < 1)
        {
            loadScreen.alpha += fadeStep;
            yield return new WaitForSeconds(0.01f);
        }

        //Load new map
        WorldManager.instance.newMap();

        int waitTime = 0;

        while (waitTime < 0)
        {
            loadTextAnimation();
            waitTime += 1;
            yield return new WaitForSeconds(0.4f);
        }

        //Fade out screen
        while (loadScreen.alpha > 0)
        {
            loadScreen.alpha -= fadeStep;
            yield return new WaitForSeconds(0.01f);
        }

        loadScreen.gameObject.SetActive(false);
        Player.instance.setInMenu(false);
    }
}
