using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public DropableItem[] spawnableItems;

    // Use this for initialization
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void spawnItem()
    {
        foreach(DropableItem i in spawnableItems)
        {
            float itemSpawn = Random.Range(0, 1);
            if (itemSpawn >= i.dropRate)
            {
                i.item.instantiate(transform.position, transform.parent);
                Destroy(this.gameObject);
            }
        }
    }

}
