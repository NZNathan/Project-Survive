using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapePrefabs : MonoBehaviour {

    public static LandscapePrefabs prefabs;

    //Prefabs
    //Forest Trees
    public GameObject[] forestTrees;

    //Camp Variables
    public GameObject[] friendlyCamps;
    public GameObject[] hostileCamps;

    //Travellers Variables
    public GameObject[] friendlyTravellers;
    public GameObject[] hostileTravellers;

    //Enemy Variables
    public GameObject[] feralCrowds;

    // Use this for initialization
    void Start ()
    {
        prefabs = this;
    }
	
}
