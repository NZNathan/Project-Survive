using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public CanvasGroup blackScreen;
    private float fadeStep = 0.05f;

    public void startGame()
    {
        StartCoroutine(switchScene());
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

        SceneManager.LoadSceneAsync("Main");
    }
}
