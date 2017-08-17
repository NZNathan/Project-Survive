using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    public Image whiteBar;
    public Image bgBar;

    private Transform target;
    private float healthBarOffset = 0.7f;

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
        //If no target, target must be dead
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector2 healthBarPos = new Vector2(target.position.x, target.position.y + healthBarOffset);

        transform.position = Camera.main.WorldToScreenPoint(healthBarPos);
    }
}
