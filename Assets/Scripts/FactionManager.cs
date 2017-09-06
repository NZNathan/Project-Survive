using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour {

    public static FactionManager instance;

    [Header("Faction Relations")]
    public FactionRelations noFaction;
    public FactionRelations banditFaction;
    public FactionRelations feralFaction;

    #region Faction Relations Structure 
    [System.Serializable]
    public class FactionRelations
    {
        [Header("Hostile Factions")]
        public bool None;
        public bool Player;
        public bool Bandit;
        public bool Feral;

        public bool getRelation(int i)
        {
            if (i == 0)
                return None;
            if (i == 1)
                return Player;
            if (i == 2)
                return Bandit;
            if (i == 3)
                return Feral;

            throw new System.Exception("Unknown faction index");
        }
    }
    #endregion 

    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Checks if faction1 is hostile toward faction2
    /// </summary>
    public bool isHostile(Faction faction1, Faction faction2)
    {
        //Two characters from the same faction will always be friendly
        if (faction1 == faction2)
            return false;

        if (faction1 == Faction.None)
            return noFaction.getRelation((int) faction2);

        if (faction1 == Faction.Bandit)
            return banditFaction.getRelation((int) faction2);

        if (faction1 == Faction.Feral)
            return feralFaction.getRelation((int) faction2);

        //Errors
        if (faction1 == Faction.Player)
            throw new System.Exception("Can't check player faction relations");

        throw new System.Exception("Unknown faction");
    }
    
}

public enum Faction {None, Player, Bandit, Feral};
