using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancestor {

    //Linked list structure
    private Ancestor parent;
    private Ancestor child;

    //Stats
    private string causeOfDeath;
    public string firstName;
    private string lastName;
    private string title;
    private Sprite[] sprite; 

	public Ancestor(Ancestor parent, Player player)
    {
        this.parent = parent;

        if(parent != null)
        {
            parent.setChild(this);
        }

        firstName = player.firstName;
        lastName = player.lastName;
        sprite = player.getSprites();
    }

    public Ancestor getParent()
    {
        return parent;
    }

    public void setChild(Ancestor child)
    {
        this.child = child;
    }

    public Ancestor getChild()
    {
        return child;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
