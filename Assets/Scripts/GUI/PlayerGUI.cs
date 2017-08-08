using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

    public AbilityIcon[] abilityIcons;
    
    public void setAbilities(Ability[] abilities)
    {
        for(int i = 1; i < abilities.Length; i++)
        {
            abilityIcons[i-1].setAbility(abilities[i]);
        }
    }

    public void usedAbility(int ability)
    {
        abilityIcons[ability - 1].startCooldown();
    }

}
