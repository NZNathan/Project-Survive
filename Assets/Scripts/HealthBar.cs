using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    public Image whiteBar;
    public Image bgBar;

    private Transform target;
    private float healthBarOffset = 0.65f;

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
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 healthBarPos = new Vector2(target.position.x, target.position.y + healthBarOffset);

        transform.position = Camera.main.WorldToScreenPoint(healthBarPos);
    }
}
