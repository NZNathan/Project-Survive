using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGen : MonoBehaviour {

    Player playerBase;
    Enemy enemyBase;
    C npcBase;
    SpriteSet[] spriteSets;

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

        int i = 0;
        foreach (Object s in sprites)
        {
            Sprite[] characterSprites = Resources.LoadAll<Sprite>("Characters/" + s.name);

            SpriteSet spriteSet = new SpriteSet(characterSprites);
            spriteSets[i] = spriteSet;
            i++;
        }

    }

    //Returns a random sprite[] 
    public Sprite[] getNewSprites()
    {
        int spriteSet = Random.Range(0, spriteSets.Length); //Will never be 2

        return spriteSets[spriteSet].getSprites();
    }

    public string getFirstName()
    {
        return "Steve";
    }

    public string getLastName()
    {
        return "Stevenson";
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

}
