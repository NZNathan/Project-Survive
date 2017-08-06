using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {

    public static int maxNoTrees = 20;
    public static float width;

    private GameObject[] trees;

	// Use this for initialization
	void Start () {
		
	}

    public void setUpArea()
    {
        //Set Up Trees
        int amountOfTrees = Mathf.FloorToInt(Random.Range(0, maxNoTrees));
        trees = new GameObject[amountOfTrees];
        //Fill array with trees from pool
        placeTrees();
    }
	
	void placeTrees()
    {
        foreach (GameObject tree in trees)
        {
            tree.transform.position = new Vector2(0,0);
            tree.SetActive(true);
        }
    }
}
