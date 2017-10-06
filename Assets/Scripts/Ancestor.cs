using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancestor {

    //Linked list structure
    private Ancestor parent;
    private Ancestor child;

    //Revenge Target
    public RevengeTarget revengeTarget;

    //Stats
    private string causeOfDeath;
    public string firstName;
    private string lastName;
    private string title;
    private RuntimeAnimatorController spriteController; 

	public Ancestor(Ancestor parent, Player player)
    {
        this.parent = parent;

        if(parent != null)
        {
            parent.setChild(this);
        }

        firstName = player.firstName;
        lastName = player.lastName;
        spriteController = player.getSpriteController();

        revengeTarget = new RevengeTarget((Enemy) player.getAttacker());
    }

    public string getName()
    {
        return firstName + lastName;
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
}
