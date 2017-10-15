using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour {

    public Enemy boss;

    private BoxCollider2D bc2D;

	// Use this for initialization
	void Start ()
    {
        bc2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            bc2D.enabled = false;

            UIManager.instance.newBossGUI(boss);
            WorldManager.instance.zoomIn(boss.transform);

            //Wait till the zoom in is complete them call to lock the screen
            Invoke("lockScreen", WorldManager.instance.respawnTime * Time.timeScale);
            boss.invulnerable = false;

            MusicManager.instance.playBossMusic();
        }
    }

    private void lockScreen()
    {
        CameraFollow.cam.GetComponentInParent<CameraFollow>().setLockScreen(boss.transform);
    }
}
