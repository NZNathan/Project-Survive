using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilitySprite { DASHSTRIKE, DODGEROLL }

public class AbilityIconList : MonoBehaviour {

    public static AbilityIconList instance;

    public Icon[] icons;

    #region Icon Structure 
    [System.Serializable]
    public class Icon
    {
        public string name;
        public Sprite icon;

    }
    #endregion

    private void Start()
    {
        instance = this;
    }

    public Sprite getAbilitSprite(AbilitySprite ab)
    {
        return icons[(int) ab].icon;
    }
}
