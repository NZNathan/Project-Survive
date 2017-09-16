using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait  {

    #region Static Methods
    public static List<Trait> traits;

    public static void generateTraits()
    {
        traits = new List<Trait>();

        //Blessings
        Trait strengthBlessing = new Trait("Blessing of Strength", new BlessingEffect(CharacterStat.STR, 3));
        traits.Add(strengthBlessing);

        Trait agilityBlessing = new Trait("Blessing of Agility", new BlessingEffect(CharacterStat.AGL, 3));
        traits.Add(agilityBlessing);

        //Curses
        Trait agilityCurse = new Trait("Curse of Agility", new CurseEffect(CharacterStat.AGL, 3));
        traits.Add(agilityCurse);

        Trait strengthCurse = new Trait("Curse of Strength", new CurseEffect(CharacterStat.STR, 3));
        traits.Add(strengthCurse);
    }

    public static Trait getTrait()
    {
        int randomTrait = Random.Range(0, traits.Count);

        return traits[randomTrait];
    }

    #endregion

    string traitName;
    Effect traitEffect;

    public Trait(string name, Effect traitEffect)
    {
        traitName = name;
        this.traitEffect = traitEffect;
    }

    public void applyTrait(CMoveCombatable character)
    {
        traitEffect.applyEffect(character);
    }

}
