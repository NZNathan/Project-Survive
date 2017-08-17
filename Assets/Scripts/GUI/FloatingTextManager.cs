using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour {

    [Header("Floating Text Parent")]
    public GameObject floatingTextParent;

    [Header("Base Floating Text")]
    public FloatingText baseFloatingText;

    //Pool Variables
    private int poolSize = 10;
    private FloatingText[] pool;
    
    // Use this for initialization
    void Start ()
    {
        pool = new FloatingText[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(baseFloatingText, floatingTextParent.transform);
            pool[i].Start();
            pool[i].gameObject.SetActive(false);
        }

	}

    public void createText(GameObject target, string sentence)
    {
        checkTarget(target);
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].gameObject.SetActive(true);
                pool[i].setup(target, sentence);
                return;
            }
        }
        throw new System.Exception("No floating texts available");
    }


    //Checks to see if the target already has a floating text above them
    void checkTarget(GameObject target)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i].gameObject.activeInHierarchy && pool[i].getTarget() == target)
            {
                pool[i].StopAllCoroutines();
                pool[i].turnoff();
                return;
            }
        }
    }

    public void disable()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i].gameObject.activeInHierarchy)
            {
                pool[i].StopAllCoroutines();
                pool[i].turnoff();
            }
        }
    }

}
