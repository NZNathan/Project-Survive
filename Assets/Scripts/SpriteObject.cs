using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteObject : MonoBehaviour {

    //Components
    private SpriteRenderer spriteRenderer;

    //Optional children objects
    public SpriteObject[] children;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setTransparency(float alpha)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = alpha;
        
        spriteRenderer.color = newColor;

        //Update children
        foreach(SpriteObject so in children)
        {
            so.setTransparency(alpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * 10 * -1);
        
    }

}
