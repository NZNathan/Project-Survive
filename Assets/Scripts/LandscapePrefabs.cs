using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapePrefabs : MonoBehaviour {

    public static LandscapePrefabs prefabs;

    //Prefabs
    //Forest Trees
    public GameObject[] forestTrees;

	// Use this for initialization
	void Start ()
    {
        prefabs = this;
    }
	
}
