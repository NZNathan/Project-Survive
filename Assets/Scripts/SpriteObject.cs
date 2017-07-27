using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteObject : MonoBehaviour {

    //Components
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * 10 * -1);
    }

}
