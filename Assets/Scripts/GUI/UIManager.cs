using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    //UI Components
    private HealthBarManager healthBarManager;
    private PlayerGUI playerGUI;

    //UI Objects
    public GameObject playerGUIObject;
    public GameObject healthBarManagerObject;
    public GameObject bagGUIObject;

    // Use this for initialization
    void Start ()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        healthBarManager = GetComponent<HealthBarManager>();
        playerGUI = GetComponent<PlayerGUI>();
    }

    //--- PLAYER GUI METHODS ---
    public void usedAbility(int ability)
    {
        playerGUI.usedAbility(ability);
    }

    public void setAbilities(Ability[] abilities)
    {
        playerGUI.setAbilities(abilities);
    }

    //--- Health Bar Manager METHODS ---
    public HealthBar newHealthBar()
    {
        return healthBarManager.newHealthBar();
    }

    //--- UI CONTROL METHODS ---
    public void disableUI()
    {
        disablehealthBarManager();
        disablePlayerGUI();
        disableBagGUI();
    }

    public void enableUI()
    {
        enablehealthBarManager();
        enablePlayerGUI();
        enableBagGUI();
    }

        public void disableBagGUI()
    {
        bagGUIObject.SetActive(false);
    }

        public void enableBagGUI()
    {
        bagGUIObject.SetActive(true);
    }

    public void disablehealthBarManager()
    {
        healthBarManagerObject.SetActive(false);
    }

    public void enablehealthBarManager()
    {
        healthBarManagerObject.SetActive(true);
    }

    public void disablePlayerGUI()
    {
        playerGUIObject.SetActive(false);
    }

    public void enablePlayerGUI()
    {
        playerGUIObject.SetActive(true);
    }
}
