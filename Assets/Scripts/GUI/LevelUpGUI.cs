using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelUpGUI : MonoBehaviour {

    public GameObject levelUpWindow;
    public Text pointsText;
    public Text[] statTexts;
    public Text[] statValues;

    private int points = 5;
    private int[] statPoints;


    public void setPoints(int points)
    {
        this.points = points + Player.instance.getLevelUpPoints();

        //Reinitialize statPints array so all values are at 0
        statPoints = new int[statTexts.Length];

        levelUpWindow.gameObject.SetActive(true);

        //Set up points value text
        pointsText.text = "" + this.points;

        //Set up player stat text values on level up window
        int[] stats = Player.instance.getStats();
        for (int i = 0; i < statValues.Length; i++)
        {
            statValues[i].text = "" + stats[i];
        }
    }

    public void addPointToStat(int stat)
    {
        if (points > 0)
        {
            statPoints[stat]++;
            statTexts[stat].text = "(+" + statPoints[stat] + ")";

            points--;
            pointsText.text = "" + points;
        }
    }

    public void removePointFromStat(int stat)
    {
        if (statPoints[stat] > 0)
        {
            statPoints[stat]--;

            if(statPoints[stat] == 0)
                statTexts[stat].text = "";
            else
                statTexts[stat].text = "(+" + statPoints[stat] + ")";

            points++;
            pointsText.text = "" + points;
        }
    }

    public void closeWindow()
    {
        Player.instance.levelup(points, statPoints);

        for(int i = 0; i < statPoints.Length; i++)
        {
            statPoints[i] = 0;
            statTexts[i].text = "";
        }

        levelUpWindow.gameObject.SetActive(false);
    }
}
