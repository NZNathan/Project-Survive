using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Tooltip : MonoBehaviour {

    [Header("UI Elements")]
    public GameObject tooltip;
    public Text itemName;
    public Text itemDescription;
    public Text sellPrice;

    [Header("Positioning")]
    public float xOffest = 0;
    public float yOffeset = 0;

    public bool tooltipOn = false;

    private void Start()
    {
        xOffest = Screen.width * (xOffest / 1920); //Normalize value for screen resolution
        yOffeset = Screen.height * (yOffeset / 1080); //divide by 1920 and 1080 as thats the resolution i built the ui for
    }

    /// <summary>
    /// Create a new tooltip with the title string, body string and price passed in
    /// </summary>
    public void newTooltip(string itemName, string itemDesc, int price)
    {
        tooltip.SetActive(true);
        this.itemName.text = itemName;
        itemDescription.text = itemDesc;
        sellPrice.text = "Sell: " + price + "G";

        tooltipOn = true;
    }

    public void closeTooltip()
    {
        tooltip.SetActive(false);
        tooltipOn = false;
    }

    private void Update()
    {
        if (tooltipOn)
        {
            Vector3 mousePos = Input.mousePosition;
            
            tooltip.transform.position = new Vector3(mousePos.x + xOffest, mousePos.y + yOffeset, mousePos.z);
        }
    }

}
