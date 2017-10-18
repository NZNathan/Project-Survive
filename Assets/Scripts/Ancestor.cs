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

        Enemy killer = (Enemy)player.getAttacker();

        killRevengeTarget(killer);

        killer.levelup(2);

        revengeTarget = new RevengeTarget(killer);
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

    /// <summary>
    /// Takes an enemy, and if they are a previous revenge target, it sets them to dead
    /// </summary>
    /// <param name="enemy"></param>
    public void killRevengeTarget(Enemy enemy)
    {
        Ancestor an = this;
        RevengeTarget rt = new RevengeTarget(enemy);

        while(an != null)
        {
            if (an.revengeTarget != null && an.revengeTarget.equals(rt))
            {
                an.revengeTarget.dead = true;
                return;
            }

            an = an.parent;
        }
    }
}
