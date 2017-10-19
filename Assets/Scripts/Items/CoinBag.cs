using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : Item
{
    public int coinAmount = 5;
    private bool withinRange = false;

    public void use()
    {
        Player.instance.addCoins(coinAmount);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
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

        if (eKeyDown && withinRange)
            use();
    }

}
