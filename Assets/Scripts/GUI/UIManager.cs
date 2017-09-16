using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    //UI Components
    private FloatingTextManager floatingTextManager;
    private HealthBarManager healthBarManager;
    private PlayerGUI playerGUI;
    private ItemPopup itemPopup;
    private BossGUI bossGUI;
    private LoadScreen loadScreen;
    public Tooltip tooltip;
    public LevelUpGUI levelUpWindow;
    public shopSystem shopWindow;

    //UI Objects
    public GameObject floatingTextManagerObject;
    public GameObject playerGUIObject;
    public GameObject healthBarManagerObject;
    public GameObject itemPopupObject;
    public GameObject bagGUIObject;
    public GameObject bossGUIObject;

    // Use this for initialization
    void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;

        floatingTextManager = GetComponent<FloatingTextManager>();
        healthBarManager = GetComponent<HealthBarManager>();
        playerGUI = GetComponent<PlayerGUI>();
        itemPopup = itemPopupObject.GetComponent<ItemPopup>();
        loadScreen = GetComponent<LoadScreen>();
        bossGUI = GetComponent<BossGUI>();
    }

    #region ---- PLAYER GUI METHODS ----
    public void addXp(float xp, float xpCap)
    {
        playerGUI.addXp(xp, xpCap);
    }

    public void usedAbility(int ability)
    {
        playerGUI.usedAbility(ability);
    }

    public void setAbilities(Ability[] abilities)
    {
        playerGUI.setAbilities(abilities);
    }
#endregion

    #region ---- Health Bar Manager METHODS ----
    public HealthBar newHealthBar()
    {
        return healthBarManager.newHealthBar();
    }
#endregion

    #region ---- Floating Text Manager METHODS ----
    public FloatingText newTextMessage(GameObject talker, string sentence)
    {
        return floatingTextManager.createText(talker, sentence);
    }
#endregion

    #region ---- Item Popup METHODS ----
    public void newPopup(GameObject target)
    {
        itemPopupObject.SetActive(true);
        itemPopup.newPopup(target);
    }

    public void closePopup()
    {
        itemPopupObject.SetActive(false);
        //itemPopup.closePopup();
    }
#endregion

    #region ---- Tooltip METHODS ----
    public void newTooltip(string title, string bodyText, int price)
    {
        tooltip.newTooltip(title, bodyText, price);
    }

    public void closeTooltip()
    {
        tooltip.closeTooltip();
    }
    #endregion

    #region ---- Level Up Window METHODS ----
    public void toggleLevelUpWindow()
    {
        if (levelUpWindow.levelUpWindow.activeInHierarchy)
            levelUpWindow.closeWindow();
        else
            levelUpWindow.openWindow();
    }
    #endregion

    #region ---- Boss GUI METHODS ----
    public void newBossGUI(CMoveCombatable boss)
    {
        bossGUI.setup(boss);
    }

    public void closeBossGUI()
    {
        bossGUI.closeGUI();
    }
    #endregion

    #region ---- Load Screen METHODS ----
    public void newLoadScreen()
    {
        loadScreen.activate();
    }
#endregion

    #region ---- Shop System METHODS ----
    public void newShopWindow()
    {
        shopWindow.openShopWindow();
    }

    public void closeShopWindow()
    {
        shopWindow.closeShopWindow();
    }
#endregion

    #region UI Control Methods
    //--- UI CONTROL METHODS ---
    public void disableUI()
    {
        disablehealthBarManager();
        disablefloatingTextManager();
        disablePlayerGUI();
        disableBagGUI();
        disableBossGUI();
    }

    public void enableUI()
    {
        enablehealthBarManager();
        enablefloatingTextManager();
        enablePlayerGUI();
        enableBagGUI();
        enableBossGUI();
    }

    public void disableBossGUI()
    {
        bossGUIObject.SetActive(false);
    }

    public void enableBossGUI()
    {
        bossGUIObject.SetActive(true);
    }

    public void disableItemPopupGUI()
    {
        itemPopupObject.SetActive(false);
    }

    public void enableItemPopupGUI()
    {
        itemPopupObject.SetActive(true);
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

    public void disablefloatingTextManager()
    {
        floatingTextManager.disable();
        floatingTextManagerObject.SetActive(false);
    }

    public void enablefloatingTextManager()
    {
        floatingTextManagerObject.SetActive(true);
    }

    public void disablePlayerGUI()
    {
        playerGUIObject.SetActive(false);
    }

    public void enablePlayerGUI()
    {
        playerGUIObject.SetActive(true);
    }
#endregion
}
