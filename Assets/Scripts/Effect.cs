using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

    void applyEffect(CMoveCombatable character);
	
}

public class BlessingEffect : Effect
{

    CharacterStat blessedStat;
    int blessingStrength;

    public BlessingEffect(CharacterStat stat, int blessingAmount)
    {
        blessedStat = stat;
        blessingStrength = blessingAmount;
    }

    public void applyEffect(CMoveCombatable character)
    {
        character.addToStat(blessedStat, blessingStrength);
    }

}

public class CurseEffect : Effect
{

    CharacterStat cursedStat;
    int curseStrength;

    public CurseEffect(CharacterStat stat, int curseAmount)
    {
        cursedStat = stat;
        curseStrength = curseAmount;
    }

    public void applyEffect(CMoveCombatable character)
    {
        character.addToStat(cursedStat, -curseStrength);
    }

}
