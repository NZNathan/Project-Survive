using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour {

    public GameObject baseArea;

    private GameObject[] areas;
    private int currentArea = 0;

    public float areaWidth = 21.4f;

    // Use this for initialization
    void Start()
    {
        areas = new GameObject[20];

        for (int i = 0; i < 10; i++)
        {
            areas[currentArea] = Instantiate(baseArea, new Vector3(areaWidth * currentArea, 0, 0), Quaternion.identity);

            currentArea++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
