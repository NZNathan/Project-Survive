using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapePrefabs : MonoBehaviour {

    public static LandscapePrefabs prefabs;

    //Prefabs

    //Town Prefabs
    public GameObject[] towns;

    //Transition Prefabs
    public GameObject[] transitions;

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

    //Fight Variables
    public GameObject[] fightCrowds;

    // Use this for initialization
    void Start ()
    {
        prefabs = this;
    }

    public GameObject getTown()
    {
        int i = Random.Range(0, towns.Length);
        return towns[i];
    }

    public GameObject getTransition()
    {
        int i = Random.Range(0, transitions.Length);
        return transitions[i];
    }

}
