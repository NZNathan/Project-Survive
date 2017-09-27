using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropableItem  {

    //public string name; Adding a name var to Serializable in an array in the inspector changes its name to match from element 0 to the name provided

    public Item item;

    [Range(0f, 1.0f)]
    public float dropRate;

    public void dropItem(Vector3 spawnPos, Transform parent)
    {
        float drop = Random.Range(0f, 1f);

        if (dropRate > drop)
            item.instantiate(spawnPos, parent);

    }

}
