using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour {

    public GameObject baseArea;
    public GameObject player;

    private GameObject[] areas;
    private int currentArea = 0;

    private float lastAreaPos;
    public float areaWidth = 21.4f;

    // Use this for initialization
    void Start()
    {
        areas = new GameObject[20];
        generateNewAreas(1);

    }

    void generateNewAreas(int count)
    {
        for (int i = 0; i < count; i++)
        {
            areas[currentArea] = Instantiate(baseArea, new Vector3(areaWidth * currentArea, 0, 0), Quaternion.identity);

            currentArea++;
            lastAreaPos = areaWidth * currentArea;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

		if(player.transform.position.x > lastAreaPos - areaWidth)
        {
            generateNewAreas(1);
            Debug.Log("New areas!");
        }
	}
}
