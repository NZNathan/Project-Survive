using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGen : MonoBehaviour {

    Character playerBase;
    Character npcBase;
    SpriteSet[] spriteSets;

	// Use this for initialization
	void Start ()
    {
        //Load in base Prefabs
        npcBase = Resources.Load<Character>("Prefabs/NPC");

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

    public Sprite[] getNewSprites()
    {
        int spriteSet = Random.Range(0, spriteSets.Length); //Will never be 2

        return spriteSets[spriteSet].getSprites();
    }

    public void createNewNPC(Transform parent, Vector2 spawnPoint)
    {
        Character npc = Instantiate(npcBase, spawnPoint, Quaternion.identity);

        //Set Parent and then reset position in relation to parent
        npc.transform.SetParent(parent);
        npc.transform.localPosition = spawnPoint;

        //Change the new NPCs look
        npc.setSpriteSet(getNewSprites());
    }

}
