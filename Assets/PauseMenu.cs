using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public void openPauseMenu()
    {
        Player.instance.setInMenu(true);
        WorldManager.isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void closePauseMenu()
    {
        Player.instance.setInMenu(false);
        WorldManager.isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void openOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void closeOptionsMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeInHierarchy)
            openPauseMenu();
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu.activeInHierarchy)
            openPauseMenu();
    }
}
