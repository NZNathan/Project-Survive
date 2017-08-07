using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {

    public static int maxNoTrees = 20;
    public static float width;

    private GameObject[] trees;

	// Use this for initialization
	void Start ()
    {
        /*
         * Node root = new Node("Safe")
         * Node people = new Node("People", 0.5);
         * Node faction = new Node("Faction", 0.5);
         * 
         * root.add(people)
         * root.add(faction)
         * 
         * Node monving = new Node("monving", 0.9);
         * Node type = new Node("type", 0.1);
         * 
         * ...
         * 
         * if(random < leftChild.probability)
         *     return leftChild.recursive
         * else
         *     return rightChild.recursive
         *     
         * leaf(gameobject[] listOfPrefabs that match the leaf's type (e.g list of safe camps or forests)
        */
    }

    public void setUpArea()
    {
        //Set Up Trees
        int amountOfTrees = Random.Range(1, maxNoTrees);
        trees = new GameObject[amountOfTrees];
        placeTrees();
    }
	
	void placeTrees()
    {
        for (int i = 0; i < trees.Length; i++)
        {
            float xPos = Random.Range(-9f, 9f);
            float yPos = Random.Range(-2f, 2f);
            trees[i] = Instantiate(LandscapeGen.prefabs.forestTrees[0], Vector2.zero, Quaternion.identity);

            //Set Parent and then reset position in relation to parent
            trees[i].transform.SetParent(transform);
            trees[i].transform.localPosition = new Vector2(xPos,yPos);
        }
    }
}
