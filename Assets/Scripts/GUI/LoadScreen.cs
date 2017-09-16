using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreen : MonoBehaviour {

    public CanvasGroup loadScreen;
    public Text levelText;
    public Text loadText;

    public static bool loading = false;

    //Animation Variables
    private int count = 0;
    private float fadeStep = 0.05f;

    private void Start()
    {
        activate();
    }

    public void activate()
    {
        if (!loading)
        {
            StartCoroutine("transitionToNewMap");
            loading = true;
        }
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
        if(Player.instance != null)
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
        if (Player.instance != null)
            Player.instance.transform.position = Player.spawmPos;

            int waitTime = 0;

        while (waitTime < 1)
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
        loading = false;
    }
}
