using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //Singleton
    public static WorldManager instance;

    //Boundaries
    public static float lowerBoundary = -3.8f;
    public static float upperBoundary = 3.8f;

    //Camera
    private CameraFollow cam;

    //Player
    private Player currentPlayer;
    private Ancestor headAncestor;
    private Ancestor tailAncestor;

    //Spawn
    public LandscapeGen landscapeGen;
    public SpriteGen spriteGenerator;
    private float respawnTime = 4f;
    public static int mapLevel = 1;

    //Dialogue
    [HideInInspector]
    public BanterGenerator banterGen;

    public float slowMotionScale = 0.2f;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        banterGen = new BanterGenerator();
        currentPlayer = spriteGenerator.createNewPlayer();
        cam = Camera.main.GetComponentInParent<CameraFollow>();
    }

    public void zoomIn(Transform t)
    {
        //Disable the UI
        UIManager.instance.disableUI();

        //Start slow motion
        slowTime();

        //Zoom to revenge Target
        cam.setZoom(CameraFollow.revengeZoom, t);

        Invoke("zoomOut", respawnTime * Time.timeScale);
    }

    public void stopTime()
    {
        //Disable shake or it will keep going when game is in paused state
        CameraFollow.cam.GetComponentInParent<CameraShake>().enabled = false;

        //Start slow motion
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0; //Scale physics time 0.02f is default value so times it by that to remain to the same scale as time

    }

    public void slowTime()
    {
        //Start slow motion
        Time.timeScale = slowMotionScale; //Scales time
        Time.fixedDeltaTime = slowMotionScale * 0.02f; //Scale physics time 0.02f is default value so times it by that to remain to the same scale as time

    }

    public void normalTime()
    {
        CameraFollow.cam.GetComponentInParent<CameraShake>().enabled = true;

        //Stop slow motion
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void zoomOut()
    {
        //Stop slow motion
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if(Player.instance != null)
            cam.resetCamera(Player.instance.transform);

        //Enable the UI
        UIManager.instance.enableUI();
    }

    public RevengeTarget getRevengeTarget()
    {
        if (headAncestor == null)
        {
            Debug.Log("Returning null");
            return null;
        }

        Debug.Log("Returning " + headAncestor.revengeTarget);

        return headAncestor.revengeTarget;
    }

    public void playerDied(Player player)
    {
        player.bag.closeBag();

        //Turn off Boss UI in case its on
        UIManager.instance.closeBossGUI();

        zoomIn(player.getAttacker().transform);

        //Craete a new Ancestor
        tailAncestor = new Ancestor(tailAncestor, player);

        if (headAncestor == null)
            headAncestor = tailAncestor;

        Ancestor an = headAncestor;
        while (an != null)
        {
            if(an.revengeTarget != null)
                Debug.Log("Ancestor: " + an.getName() + ", killed by " + an.revengeTarget.firstName + " " + an.revengeTarget.lastName);
            else
                Debug.Log("Ancestor: " + an.getName() + ", killed by null");

            an = an.getChild();
        }

        Invoke("resetPlayer", respawnTime * Time.timeScale);
    }

    public void newMap()
    {
        cam.resetCamera(currentPlayer.transform);

        landscapeGen.resetLandscape();

        //Enable the UI
        UIManager.instance.enableUI();
    }

    private void resetPlayer()
    {
        normalTime();

        currentPlayer = spriteGenerator.createNewPlayer();

        currentPlayer.gameObject.name = "Player";

        UIManager.instance.newLoadScreen();
    }

}
