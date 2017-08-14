using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour {

    //Prefabs
    public static LandscapePrefabs prefabs;
    public Area baseArea;
    public GameObject player;
    public DT scenarioTree;

    //Sprite Gen Variables
    public SpriteGen spriteGenerator;

    //Area Gen Variables
    public static int levelSize = 21;
    private Area[] areas;
    
    //Area Spawn Location Variables
    private int currentArea = 0;
    private float lastAreaPos;
    private float firstAreaPos;
    public float areaWidth = 21.4f;

    // Use this for initialization
    void Start()
    {
        prefabs = GetComponent<LandscapePrefabs>();
        scenarioTree = GetComponent<DT>();
        scenarioTree.createTree();

        areas = new Area[levelSize];
        generateNewAreas();

        Area.width = areaWidth;

    }

    public Area getFirstArea()
    {
        return areas[0];
    }

    public void resetLandscape(Player player)
    {
        Area respawnArea = Instantiate(areas[0], new Vector3(0, 0, 0), Quaternion.identity);
        respawnArea.name = "Respawned Area";

        //Delete and reset all other areas
        deleteLandscape();
        areas = new Area[levelSize];
        areas[0] = respawnArea;
        generateNewAreas();

        this.player = player.gameObject;
    }

    void deleteLandscape()
    {
        for (int i = 0; i < levelSize; i++)
        {
            Destroy(areas[i].gameObject);
        }
    }

    void generateNewAreas()
    {
        float areaPos = 0f;
        
        for (int i = 0; i < levelSize; i++)
        {
            if(areas[i] != null)
            {
                areaPos += areaWidth;
                continue;
            }

            areas[i] = Instantiate(baseArea, new Vector3(areaPos, 0, 0), Quaternion.identity);

            areas[i].setUpArea();
            if (i > 0)
            {
                //Get random scenario and instantiate it
                GameObject scenario = Instantiate(scenarioTree.getScenario(), new Vector3(areaPos, 0, 0), Quaternion.identity);
                scenario.transform.SetParent(areas[i].transform);

                //Generation all Characters in scenario
                C[] characters = scenario.GetComponentsInChildren<C>();
                spriteGenerator.generateCharacters(characters);
            }

            //Generate Sprites
            //spriteGenerator.createNewNPC(areas[i].transform, new Vector2(-10,0));
            //spriteGenerator.createNewNPC(areas[i].transform, new Vector2(-10, 2));

            //Turn off area
            areas[i].gameObject.SetActive(false);

            //Increase the current area int and update the new area pos of the last area on the map
            areaPos += areaWidth;
        }

        currentArea = -1;
        lastAreaPos = -areaWidth;
        firstAreaPos = -areaWidth * 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
            return;

        //Move into a invoke repeating method so it can be stopped and more efficient?
		if(player.transform.position.x > lastAreaPos && currentArea <= levelSize-2)
        {
            //Set active the next area in front of the player and disable the one two areas behind
            areas[currentArea+1].gameObject.SetActive(true);

            if (currentArea >= 2)
                areas[currentArea - 2].gameObject.SetActive(false);

            if (currentArea + 2 != levelSize)
            {
                currentArea++;
                lastAreaPos += areaWidth;
                firstAreaPos += areaWidth;
            }
        }
        else if (player.transform.position.x < firstAreaPos && currentArea > 1)
        {
            //Set active the next area behind of the player and disable the one two areas ahead
            areas[currentArea - 2].gameObject.SetActive(true);

            areas[currentArea+1].gameObject.SetActive(false);

            currentArea--;
            lastAreaPos -= areaWidth;
            firstAreaPos -= areaWidth;
        }
    }
}
