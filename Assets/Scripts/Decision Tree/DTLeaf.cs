using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTLeaf : DTNode {

    GameObject[] prefabs;

    public void setUpDTNode(string name)
    {
        nodeName = name;
    }

    public void setPrefab(GameObject[] prefabs)
    {
        this.prefabs = prefabs;
    }

    public override GameObject getLeaf()
    {
        int i = Random.Range(0, prefabs.Length);
        return prefabs[i];
    }
}
