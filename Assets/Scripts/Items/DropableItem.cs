using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropableItem  {

    public Item item;

    [Range(0f, 1.0f)]
    public float dropRate;

    public void dropItem(Vector3 spawnPos)
    {
        float drop = UnityEngine.Random.Range(0f, 1f);

        if (dropRate > drop)
            item.instantiate(spawnPos);

    }
}
