using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //Singleton
    public static WorldManager instance;

    //Player
    private Player playerPrefab;
    private Player currentPlayer;
    private Ancestor headAncestor;
    private Ancestor tailAncestor;

    //Spawn
    private LandscapeGen landscapeGen;
    private SpriteGen spriteGenerator;
    private GameObject lastCheckpoint;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        landscapeGen = GameObject.Find("LandscapeManager").GetComponent<LandscapeGen>();
        spriteGenerator = GameObject.Find("CharacterManager").GetComponent<SpriteGen>();
        playerPrefab = Resources.Load<Player>("Prefabs/Player");

        lastCheckpoint = GameObject.Find("Area(Clone)");
    }

    public void playerDied(Player player)
    {
        tailAncestor = new Ancestor(tailAncestor, player);

        if (headAncestor == null)
            headAncestor = tailAncestor;

        resetPlayer();
    }

    private void resetPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        currentPlayer.GetComponent<C>().setSpriteSet(spriteGenerator.getNewSprites());
        currentPlayer.name = "Player";

        landscapeGen.resetLandscape(lastCheckpoint, currentPlayer);
        lastCheckpoint = landscapeGen.getFirstArea(); //FIXME: Instatiates it as it was in game world (npcs current positions not their starting ones)

        Camera.main.GetComponentInParent<CameraFollow>().setTarget(currentPlayer.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
