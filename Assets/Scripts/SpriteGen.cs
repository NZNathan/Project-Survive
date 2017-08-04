using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SpriteGen : MonoBehaviour {

    Player playerBase;
    Enemy enemyBase;
    C npcBase;
    SpriteSet[] spriteSets;

    string[] firstNames;
    string[] lastNames;

	// Use this for initialization
	void Start ()
    {
        //Load in base Prefabs
        playerBase = Resources.Load<Player>("Prefabs/Player");
        enemyBase = Resources.Load<Enemy>("Prefabs/Feral");
        npcBase = Resources.Load<C>("Prefabs/NPC");

        //Load in Sprites
        Object[] sprites;

        sprites = Resources.LoadAll<Texture2D>("Characters");

         //Set size of array to the amount of characters that are in the characters folder
        spriteSets = new SpriteSet[sprites.Length];

        //Load in all sprites that will be used to generate all characters
        int i = 0;
        foreach (Object s in sprites)
        {
            Sprite[] characterSprites = Resources.LoadAll<Sprite>("Characters/" + s.name);

            SpriteSet spriteSet = new SpriteSet(characterSprites);
            spriteSets[i] = spriteSet;
            i++;
        }

        //Read in the names from the name files
        readInNames();
    }

    //Returns a random sprite[] 
    public Sprite[] getNewSprites()
    {
        int spriteSet = Random.Range(0, spriteSets.Length); //Will never be 2

        return spriteSets[spriteSet].getSprites();
    }

    public string getFirstName()
    {
        int random = Mathf.FloorToInt(Random.Range(0, firstNames.Length));
        return firstNames[random];
    }

    public string getLastName()
    {
        int random = Mathf.FloorToInt(Random.Range(0, lastNames.Length));
        return lastNames[random];
    }

    public Player createNewPlayer()
    {
        Player p = Instantiate(playerBase, new Vector3(0, 0, 0), Quaternion.identity);

        p.setSpriteSet(getNewSprites());

        //Set Player stats and details
        p.firstName = getFirstName();
        p.lastName = getLastName();

        return p;
    }

    public void createNewNPC(Transform parent, Vector2 spawnPoint)
    {
        C npc = Instantiate(npcBase, spawnPoint, Quaternion.identity);

        //Set Parent and then reset position in relation to parent
        npc.transform.SetParent(parent);
        npc.transform.localPosition = spawnPoint;

        //Change the new NPCs look
        npc.setSpriteSet(getNewSprites());

        //Set NPC stats and details
        npc.firstName = getFirstName();
        npc.lastName = getLastName();
    }

    public void createNewEnemy(Transform parent, Vector2 spawnPoint)
    {
        Enemy enemy = Instantiate(enemyBase, spawnPoint, Quaternion.identity);

        //Set Parent and then reset position in relation to parent
        enemy.transform.SetParent(parent);
        enemy.transform.localPosition = spawnPoint;

        //Change the new NPCs look
        enemy.setSpriteSet(getNewSprites());

        //Set NPC stats and details
        enemy.firstName = getFirstName();
        enemy.lastName = getLastName();
    }

    private void readInNames()
    {
        FileReader fileReader = new FileReader();

        firstNames = fileReader.listToArray(fileReader.readFile("firstnames.txt"));

        lastNames = fileReader.listToArray(fileReader.readFile("lastnames.txt"));
    }

}
