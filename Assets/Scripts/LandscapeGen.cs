using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeGen : MonoBehaviour {

    //Prefabs
    public GameObject baseArea;
    public GameObject player;

    //Sprite Gen Variables
    private SpriteGen spriteGenerator;

    //Area Gen Variables
    private GameObject[] areas;
    private int currentArea = 0;

    //Area Spawn Location Variables
    private float lastAreaPos;
    public float areaWidth = 21.4f;

    // Use this for initialization
    void Start()
    {
        spriteGenerator = GameObject.Find("CharacterManager").GetComponent<SpriteGen>();

        areas = new GameObject[20];
        generateNewAreas(1);

        //Generate random player sprite on start of game
        player.GetComponent<C>().setSpriteSet(spriteGenerator.getNewSprites());

    }

    public GameObject getFirstArea()
    {
        return areas[0];
    }

    public void resetLandscape(GameObject startPoint, Player player)
    {
        GameObject respawnArea = Instantiate(startPoint, new Vector3(0, 0, 0), Quaternion.identity);
        respawnArea.name = "Respawned Area";

        //Delete and reset all other areas
        deleteLandscape();
        areas = new GameObject[20];
        areas[0] = respawnArea;
        currentArea = 1;
        lastAreaPos = areaWidth * currentArea;

        this.player = player.gameObject;
    }

    void deleteLandscape()
    {
        for (int i = 0; i < currentArea; i++)
        {
            Destroy(areas[i]);
        }
    }

    void generateNewAreas(int count)
    {
        for (int i = 0; i < count; i++)
        {
            areas[currentArea] = Instantiate(baseArea, new Vector3(lastAreaPos, 0, 0), Quaternion.identity);

            //Generate Sprites
            spriteGenerator.createNewNPC(areas[currentArea].transform, new Vector2(-10,0));
            spriteGenerator.createNewNPC(areas[currentArea].transform, new Vector2(-10, 2));

            spriteGenerator.createNewEnemy(areas[currentArea].transform, new Vector2(-2, 0));

            //Increase the current area int and update the new area pos of the last area on the map
            currentArea++;
            lastAreaPos = areaWidth * currentArea;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
            return;

        //Move into a invoke repeating method so it can be stopped and more efficient?
		if(player.transform.position.x > lastAreaPos - areaWidth)
        {
            generateNewAreas(1);
        }
	}
}
