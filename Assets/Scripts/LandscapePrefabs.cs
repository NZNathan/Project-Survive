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

    //Empty Houses
    public GameObject[] emptyHouses;

    //Enemy Variables
    public GameObject[] feralCrowds;

    //Fight Variables
    public GameObject[] fightCrowds;

    //Boss Variables
    public GameObject[] bossAreas;

    // Use this for initialization
    void Start ()
    {
        prefabs = this;
    }

    public GameObject getBossArea()
    {
        int i = Random.Range(0, bossAreas.Length);
        return bossAreas[i];
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
