using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public ItemAbility itemAbility;

    [Range(0f, 1.0f)]
    public float dropRate;

	// Use this for initialization
	void Start ()
    {
        itemAbility = GetComponent<ItemAbility>();
	}

    public void useItem()
    {
        itemAbility.useItem();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player.instance.itemEnterProximity(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player.instance.itemLeaveProximity(this);
        }
    }

}
