using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //Singleton
    public static WorldManager instance;

    //Camera
    private CameraFollow cam;

    //Player
    private Player playerPrefab;
    private Player currentPlayer;
    private Ancestor headAncestor;
    private Ancestor tailAncestor;

    //Spawn
    private LandscapeGen landscapeGen;
    private SpriteGen spriteGenerator;
    private GameObject lastCheckpoint;
    private float respawnTime = 3f;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        landscapeGen = GameObject.Find("LandscapeManager").GetComponent<LandscapeGen>();
        spriteGenerator = GameObject.Find("CharacterManager").GetComponent<SpriteGen>();
        playerPrefab = Resources.Load<Player>("Prefabs/Player");

        cam = Camera.main.GetComponentInParent<CameraFollow>();
        lastCheckpoint = GameObject.Find("Area(Clone)");

    }

    public void playerDied(Player player)
    {
        //Zoom to revenge Target
        Transform t = player.getAttacker().transform;
        cam.setZoom(CameraFollow.revengeZoom, t);

        //Craete a new Ancestor
        tailAncestor = new Ancestor(tailAncestor, player);

        if (headAncestor == null)
            headAncestor = tailAncestor;

        Invoke("resetPlayer", respawnTime);
    }

    private void resetPlayer()
    {
        currentPlayer = spriteGenerator.createNewPlayer();

        currentPlayer.name = "Player";
        landscapeGen.resetLandscape(lastCheckpoint, currentPlayer);
        lastCheckpoint = landscapeGen.getFirstArea(); //FIXME: Instatiates it as it was in game world (npcs current positions not their starting ones)

        cam.resetCamera(currentPlayer.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
