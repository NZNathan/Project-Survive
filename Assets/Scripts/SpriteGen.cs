using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGen : MonoBehaviour {

    Character npcBase;
    SpriteSet[] spriteSets;

	// Use this for initialization
	void Start ()
    {
        //Load in base Prefabs
        npcBase = Resources.Load<Character>("Prefabs/NPC");
        Debug.Log(npcBase);
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

        createNewNPC();

    }

    void createNewNPC()
    {
        Character npc = Instantiate(npcBase, new Vector3(-22,0,0), Quaternion.identity);
        npc.setSprites(spriteSets[1].getSprites());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
