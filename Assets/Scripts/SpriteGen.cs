using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SpriteGen : MonoBehaviour {

    //Singleton
    public static SpriteGen instance;

    //Base Prefabs
    public Player playerBase;
    public Enemy enemyBase;
    public C npcBase;

    //Sprites
    public RuntimeAnimatorController[] playerControllers;
    public RuntimeAnimatorController[] NPCControllers;
    public RuntimeAnimatorController[] feralControllers;
    public RuntimeAnimatorController[] banditControllers;

    //Details
    public static string[] firstNames;
    public static string[] lastNames;

    //Stats


	// Use this for initialization
	void Start ()
    {
        instance = this;
        //Generate Traits
        Trait.generateTraits();

        //Read in the names from the name files
        readInNames();
    }

    //Returns a random sprite[] 
    public RuntimeAnimatorController getSpriteController(C character)
    {
        RuntimeAnimatorController[] spriteControllers;

        switch (character.faction)
        {
            case (Faction.Player):
                spriteControllers = playerControllers;
                break;
            case (Faction.None):
                spriteControllers = NPCControllers;
                break;
            case (Faction.Bandit):
                spriteControllers = banditControllers;
                break;
            default:
                spriteControllers = feralControllers;
                break;
        }
        

        int spriteSet = Random.Range(0, spriteControllers.Length); //Will never be 2

        return spriteControllers[spriteSet];
    }

    public static string getFirstName()
    {
        int random = Mathf.FloorToInt(Random.Range(0, firstNames.Length));
        return firstNames[random];
    }

    public static string getLastName()
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
                character.setSpriteController(getSpriteController(character));

                //Set NPC stats and details
                character.firstName = getFirstName();
                character.lastName = getLastName();
            }
        }
    }

    public void generateBoss(Spawner spawner)
    {
       spawner.spawnBoss(WorldManager.instance.getRevengeTarget());
    }

    public Player createNewPlayer()
    {
        Player p = Instantiate(playerBase, Player.spawmPos, Quaternion.identity);

        p.chooseRandomClass();

        p.setSpriteController(getSpriteController(p));

        //Set Player stats and details
        p.firstName = getFirstName();

        //Set up last name to be the family name
        if (Player.familyName == "")
            p.lastName = getLastName();
        else
            p.lastName = Player.familyName;

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
        npc.setSpriteController(getSpriteController(npc));
       
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

    public void generateEnemy(Enemy enemy)
    {
        //Change the new NPCs look
        enemy.setSpriteController(getSpriteController(enemy));

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
