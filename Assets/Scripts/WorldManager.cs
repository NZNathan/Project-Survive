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

    //Pause
    public static bool isPaused = false;

    //Camera
    private CameraFollow cam;

    //Player
    private Player currentPlayer;
    private Ancestor headAncestor;
    private Ancestor tailAncestor;

    //Spawn
    public LandscapeGen landscapeGen;
    public SpriteGen spriteGenerator;
    public float respawnTime = 4f;
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
        Cursor.visible = false;
    }

    public void zoomIn(Transform t)
    {
        //Disable the UI
        UIManager.instance.disableUI();

        //Start slow motion and pause game
        isPaused = true;
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
        isPaused = false;

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
        if (tailAncestor == null)
        {
            if (headAncestor == null || headAncestor.revengeTarget.dead)
                return null;
            else
                return headAncestor.revengeTarget;
        }

        Ancestor an = tailAncestor;

        while (an != null)
        {
            if (!an.revengeTarget.dead)
                return an.revengeTarget;

            an = an.getParent();
        }

        //Return null if all revenge targets are dead
        return null;
    }

    public void killRevengeTarget(Enemy enemy)
    {
        if (tailAncestor != null)
            tailAncestor.killRevengeTarget(enemy);

        else if(headAncestor != null)
            headAncestor.killRevengeTarget(enemy);
    }

    public void playerDied(Player player)
    {
        player.bag.closeBag();

        //Turn off Boss UI in case its on
        UIManager.instance.closeBossGUI();

        CameraFollow.screenLocked = false;

        MusicManager.instance.stopBossMusic();

        zoomIn(player.getAttacker().transform);

        //Craete a new Ancestor
        tailAncestor = new Ancestor(tailAncestor, player);

        if (headAncestor == null)
            headAncestor = tailAncestor;

        Invoke("deathScreen", respawnTime/2 * Time.timeScale);
    }

    public void newMap()
    {
        cam.resetCamera(currentPlayer.transform);

        landscapeGen.resetLandscape();

        //Enable the UI
        UIManager.instance.enableUI();
    }

    public void increaseLevel()
    {
        mapLevel++;
        landscapeGen.nextLevel(mapLevel);
    }

    private void deathScreen()
    {
        landscapeGen.gameObject.SetActive(false);

        normalTime();

        UIManager.instance.newDeathScreen();
    }

    public void resetPlayer(Player newPlayer)
    {
        landscapeGen.gameObject.SetActive(true);

        //Reset the equipment slot icons
        UIManager.instance.resetEquipmentSlots();
        UIManager.instance.resetXPBar();

        currentPlayer = newPlayer;

        newPlayer.setSingleton();

        newPlayer.gameObject.name = "Player";
        newPlayer.gameObject.SetActive(true);
        
        newMap();
    }

    private void Update()
    {
        if (Player.instance.isInMenu() || Player.instance.isDead())
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }

}
