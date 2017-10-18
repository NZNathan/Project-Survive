using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public string familyName;

    public CanvasGroup blackScreen;
    public GameObject nameWindow;
    public GameObject[] buttons;
    private float fadeStep = 0.05f;

    public GameObject optionsMenu;

    public void startGame()
    { 
        foreach (GameObject btn in buttons)
            btn.SetActive(false);

        nameWindow.SetActive(true);
    }

    public void closeOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void openOptions()
    {
        optionsMenu.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public IEnumerator switchScene()
    {
        MusicManager.instance.fadeMusic();
        yield return new WaitForSeconds(0.2f);
        

        blackScreen.gameObject.SetActive(true);

        //Fade in screen
        while (blackScreen.alpha < 1)
        {
            blackScreen.alpha += fadeStep;
            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadSceneAsync("Tutorial");
    }

    public void setName(string newName)
    {
        familyName = newName;
    }

    public void doneBtn()
    {
        if (familyName.Length >= 2)
        {
            Player.familyName = familyName;
            StartCoroutine(switchScene());
        }

    }
}
