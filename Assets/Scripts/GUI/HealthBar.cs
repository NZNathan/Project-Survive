using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    //Components
    public Image healthBar;
    public Image whiteBar;
    public Image bgBar;

    //Boss Variable
    public bool bossBar = false;

    //Positioning Variables
    private Transform target;
    private float healthBarOffset = 0.7f;

    //White bar Animation
    private float initialPause = 0.4f;
    private float steps = 25f; // the amount of steps for the animation

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    public void setActive(bool active)
    {
        bgBar.gameObject.SetActive(active);
        whiteBar.gameObject.SetActive(active);
        healthBar.gameObject.SetActive(active);
    }
	

	void Update ()
    {
        if (!bossBar)
        {
            //If no target, target must be dead
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            //Position the health bar above its targets head
            Vector2 healthBarPos = new Vector2(target.position.x, target.position.y + healthBarOffset);

            transform.position = CameraFollow.cam.WorldToScreenPoint(healthBarPos);
        }
    }

    public void recoverHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
        whiteBar.fillAmount = fillAmount;
    }

    public void loseHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;

        if(this.gameObject.activeInHierarchy)
            StartCoroutine(animateFill());
    }

    IEnumerator animateFill()
    {
        //Pause before starting animation
        yield return new WaitForSeconds(initialPause);

        float step = (whiteBar.fillAmount - healthBar.fillAmount) / steps;

        while (whiteBar.fillAmount >= healthBar.fillAmount)
        {
            whiteBar.fillAmount -= step;

            //If whiteBar goes to low, bring it equal to the healthbar
            if (whiteBar.fillAmount < healthBar.fillAmount)
                whiteBar.fillAmount = healthBar.fillAmount;

            yield return null;
        }
    }

}
