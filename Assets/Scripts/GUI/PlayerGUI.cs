using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

    public AbilityIcon[] abilityIcons;

    //XP Bar Variables
    private float currentXp = 0;
    private float currentXpCap = 5;
    private float step = 0.05f;
    public Image xpBar;
    
    public void addXp(float xp, float xpCap)
    {
        //xpBar.fillAmount = xp / xpCap;

        //If xp is smaller than current xp, then it must have reached the cap and needs to have the cap added to it for the animation
        if (xp < currentXp)
            xp += currentXpCap;

        StartCoroutine(animateFill(xp, xpCap));
    }

    public void resetBar()
    {
        xpBar.fillAmount = 0;
    }

    public void resetXp(int cap)
    {
        currentXp = 0;
        currentXpCap = cap;
    }

    public void setAbilities(Ability[] abilities)
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            abilityIcons[i].setAbility(abilities[i]);
        }
    }

    public void usedAbility(int ability)
    {
        abilityIcons[ability].startCooldown();
    }

    /// <summary>
    /// Goes through all abilities and starts their cooldowns again if they were paused when the UI was diabled
    /// </summary>
    public void restartCooldowns()
    {
        foreach(AbilityIcon ab in abilityIcons)
        {
            ab.restartCooldown();
        }
    }

    IEnumerator animateFill(float xp, float xpCap)
    {
        while(currentXp <= xp)
        {
            currentXp += step;

            if(currentXp >= currentXpCap)
            {
                currentXp -= currentXpCap;
                xp -= currentXpCap;

                currentXpCap = xpCap;
            }

            xpBar.fillAmount = currentXp / currentXpCap;

            yield return null;
        }
    }

}
