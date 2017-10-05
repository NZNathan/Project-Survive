using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public DropableItem[] spawnableItems;

    public void spawnItem()
    {
        
        foreach(DropableItem i in spawnableItems)
        {
            float itemSpawn = Random.Range(0f, 1f);
            if (itemSpawn <= i.dropRate)
            {
                i.item.instantiate(transform.position, transform.parent);
                this.gameObject.SetActive(false);
                return;
            }
        }
    }

}
