using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BossGUI : MonoBehaviour {

    public GameObject bossGUIWrappper;

    public HealthBar hpBar;
    public Text nameBox;

	public void setup(CMoveCombatable boss)
    {
        boss.setHealthbar(hpBar);
        nameBox.text = boss.firstName + " " + boss.lastName;

        hpBar.recoverHealth(1f);
        hpBar.setActive(true);

        bossGUIWrappper.SetActive(true);
    }

    public void closeGUI()
    {
        bossGUIWrappper.SetActive(false);
    }
}
