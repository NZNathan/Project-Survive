using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemAbility))]
public class Item : MonoBehaviour {

    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemPrice;
    public ItemAbility itemAbility;

	// Use this for initialization
	void Start ()
    {
        itemAbility = GetComponent<ItemAbility>();
	}

    public void useItem()
    {
        itemAbility.useItem();
    }

    public void instantiate(Vector3 spawnPos, Transform parent)
    {
        Item item = Instantiate(this, spawnPos, Quaternion.identity);
        item.gameObject.name = itemName;
        item.transform.SetParent(parent);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player.instance.itemsInRange.itemEnterProximity(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Player.instance.itemsInRange.itemLeaveProximity(this);
        }
    }

}
