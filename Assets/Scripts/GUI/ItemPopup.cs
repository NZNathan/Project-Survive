using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemPopup : MonoBehaviour {

    public Text textbox;
    public Image itemPopupBG;

    private float popupOffset = 0;
    private float yPadding = 0.2f;
    private Transform target;
    

    public void newPopup(GameObject target)
    {
        textbox.text = target.name;

        Sprite sp = target.GetComponent<SpriteRenderer>().sprite;
        popupOffset = sp.bounds.size.y + yPadding;

        this.target = target.transform;
    }

    public void closePopup()
    {
        itemPopupBG.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //If no target, target must be dead
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector2 popupPos = new Vector2(target.position.x, target.position.y + popupOffset);

        transform.position = CameraFollow.cam.WorldToScreenPoint(popupPos);
    }
}
