using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour {

    //Prefabs
    public static LandscapePrefabs prefabs;
    public Area baseArea;
    public DT scenarioTree;

    //Sprite Gen Variables
    public SpriteGen spriteGenerator;

    //Area Gen Variables -- Have a enum here for terrian type? (Forest, desert, grassland)
    [Range(3, 99)]
    public int levelSize = 5; //first area is a town, seond is empty, last area will always be a transition area so always need at least 3
    private Area[] areas;

    //Area Limits Variables
    public static float leftEdge;
    public static float rightEdge;

    //Area Spawn Location Variables
    private int currentArea = 0;
    private float lastAreaPos;
    private float firstAreaPos;
    public float areaWidth = 21.4f;
    private int activeRange = 2;

    // Use this for initialization
    void Start()
    {
        prefabs = GetComponent<LandscapePrefabs>();
        scenarioTree = GetComponent<DT>();
        scenarioTree.createTree();

        areas = new Area[levelSize];
        generateNewAreas();

        Area.width = areaWidth;

        rightEdge = (levelSize-2) * areaWidth - (areaWidth/2);
        leftEdge = -areaWidth*1.5f; //Need to change if town size is changed
    }

    public Area getFirstArea()
    {
        return areas[0];
    }

    public void resetLandscape(Player player)
    {
        Area respawnArea = Instantiate(areas[0], new Vector3(-areaWidth, 0, 0), Quaternion.identity);
        respawnArea.name = "Respawned Area";

        //Delete and reset all other areas
        deleteLandscape();
        areas = new Area[levelSize];
        areas[0] = respawnArea;
        generateNewAreas();

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
        float areaPos = -areaWidth;
        
        for (int i = 0; i < levelSize; i++)
        {
            if(areas[i] != null)
            {
                areaPos += areaWidth;
                continue;
            }

            areas[i] = Instantiate(baseArea, new Vector3(areaPos, 0, 0), Quaternion.identity);

            areas[i].setUpArea();

            //If i == 0 then Area is the town Area
            if(i == 0)
            {
                areas[i].gameObject.name = "Safe Zone";

                //Give town extra area to make it bigger?
                Area biggerArea = Instantiate(baseArea, new Vector3(areaPos-areaWidth, 0, 0), Quaternion.identity);
                biggerArea.transform.SetParent(areas[i].transform);

                //Get the town prefab and instantiate it, setting its parten to the area
                GameObject town = Instantiate(prefabs.getTown(), new Vector3(areaPos, 0, 0), Quaternion.identity);
                town.transform.SetParent(areas[i].transform);
            }
            //If at last area, the area will be a transition zone
            else if(i == levelSize - 1)
            {
                areas[i].gameObject.name = "Transition Zone";

                //Get the transition prefab
                GameObject transition = Instantiate(prefabs.getTransition(), new Vector3(areaPos, 0, 0), Quaternion.identity);
                transition.transform.SetParent(areas[i].transform);

                //Geneate Enemies still?
                //Generation all Characters in scenario
                Spawner[] characters = transition.GetComponentsInChildren<Spawner>();
                spriteGenerator.generateCharacters(characters);
            }
            else if (i > 1)
            {
                //Get random scenario and instantiate it
                GameObject scenario = Instantiate(scenarioTree.getScenario(), new Vector3(areaPos, 0, 0), Quaternion.identity);
                scenario.transform.SetParent(areas[i].transform);

                //Generation all Characters in scenario
                Spawner[] characters = scenario.GetComponentsInChildren<Spawner>();
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

        currentArea = 0;
        lastAreaPos = -areaWidth;
        firstAreaPos = -areaWidth*2;

        areas[0].gameObject.SetActive(true);
        areas[1].gameObject.SetActive(true);
        areas[2].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance == null)
            return;

        //Move into a invoke repeating method so it can be stopped and more efficient?
        if (Player.instance.transform.position.x > lastAreaPos && currentArea <= levelSize - 2)
        {
            //Set active the next area in front of the player and disable the one two areas behind
            areas[currentArea + activeRange].gameObject.SetActive(true);

            if (currentArea >= 1 + activeRange)
                areas[currentArea - activeRange - 1].gameObject.SetActive(false);

            if (currentArea + activeRange + 1 != levelSize)
            {
                currentArea++;
                lastAreaPos += areaWidth;
                firstAreaPos += areaWidth;
            }
        }
        else if (Player.instance.transform.position.x < firstAreaPos && currentArea > activeRange)
        {
            //Set active the next area behind of the player and disable the one two areas ahead
            areas[currentArea - activeRange - 1].gameObject.SetActive(true);

            areas[currentArea + activeRange].gameObject.SetActive(false);

            currentArea--;
            lastAreaPos -= areaWidth;
            firstAreaPos -= areaWidth;
        }
    }
}
