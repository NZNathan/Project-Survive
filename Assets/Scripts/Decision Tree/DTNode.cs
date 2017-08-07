using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTNode : ScriptableObject {

    private DTNode[] children;
    public string nodeName;
    public float prob;

    public void setUpDTNode(string name, float prob)
    {
        nodeName = name;
        this.prob = prob;
    }

    public void add(params DTNode[] childs)
    {
        children = new DTNode[childs.Length];

        for(int i = 0; i < childs.Length; i++)
        {
            children[i] = childs[i];
        }
    }

    public virtual GameObject getLeaf()
    {
        float r = Random.Range(0f, 1f);
        float childrenProb = 0;

        foreach (DTNode n in children)
        {
            childrenProb += n.prob;

            if (r <= childrenProb)
                return n.getLeaf();
        }

        Debug.Log("Shouldn't reach this point! " + nodeName);
        return null;
    }
}
