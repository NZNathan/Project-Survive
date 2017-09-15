using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SpriteGen : MonoBehaviour {

    //Base Prefabs
    public Player playerBase;
    public Enemy enemyBase;
    public C npcBase;

    //Sprites
    SpriteSet[] spriteSets;

    //Details
    string[] firstNames;
    string[] lastNames;

    //Stats


	// Use this for initialization
	void Start ()
    {
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

        //Generate Traits
        Trait.generateTraits();

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

    public void generateCharacters(Spawner[] spawners)
    {
        //TODO: Extend to treat characters differently depending on their faction and details
        foreach (Spawner spawner in spawners)
        {
            if (spawner.bossSpawn)
            {
                generateBoss(spawner);
            }
            else
            {
                //Get the spawner to spawn the prefab
                C character = spawner.spawnCharacter();

                //Change the new NPCs look
                character.setSpriteSet(getNewSprites());

                //Set NPC stats and details
                character.firstName = getFirstName();
                character.lastName = getLastName();
            }
        }
    }

    public void generateBoss(Spawner spawner)
    {
        Enemy boss = spawner.spawnBoss(WorldManager.instance.getRevengeTarget());

        //spawner.spawnBoss(boss);

        Debug.Log("Craeted Boss");
    }

    public Player createNewPlayer()
    {
        Player p = Instantiate(playerBase, new Vector3(-5f, 0, 0), Quaternion.identity);

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

        generateNPC(npc);
    }

    void generateNPC(C npc)
    {
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

        generateEnemy(enemy);
    }

    void generateEnemy(Enemy enemy)
    {
        //Change the new NPCs look
        enemy.setSpriteSet(getNewSprites());

        //Set NPC stats and details
        enemy.firstName = getFirstName();
        enemy.lastName = getLastName();
    }

    private void readInNames()
    {
        firstNames = (Resources.Load<TextAsset>("Files/firstnames")).text.Split('\n');

        lastNames = (Resources.Load<TextAsset>("Files/lastnames")).text.Split('\n');
    }

}
