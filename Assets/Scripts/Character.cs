using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Character : MonoBehaviour {

    //Components
    private SpriteRenderer[] spriteRenderers;

	// Use this for initialization
	void Start ()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		foreach(var sr in spriteRenderers)
        {
            sr.sortingOrder = (int) (transform.position.y*10 *-1);
        }
	}
}
