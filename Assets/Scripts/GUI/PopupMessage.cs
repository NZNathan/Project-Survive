using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{

    [Header("UI Elements")]
    public GameObject popup;
    public Text bodyText;

    public bool popupOn = false;

    /// <summary>
    /// Create a new tooltip with the title string, body string and price passed in
    /// </summary>
    public void newPopup(string message)
    {
        popup.SetActive(true);
        bodyText.text = message;

        popupOn = true;
    }

    public void closePopup()
    {
        popup.SetActive(false);
        popupOn = false;
    }

    private void Update()
    {
        if (popupOn)
        {
            Player.instance.setInMenu(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.instance.setInMenu(false);
                closePopup();
            }
        }
    }

}
