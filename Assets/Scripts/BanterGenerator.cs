using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanterGenerator {


    public string getPainYell()
    {
        float ran = Random.Range(0f, 1f);

        if (ran < 0.5f)
            return "Ouch!";
        else
            return "You'll regret that";
    }

    public string getAttackYell()
    {
        float ran = Random.Range(0f, 1f);

        if (ran < 0.5f)
            return "Consecutive normal slashes!";
        else
            return "Why won't you die?!";
    }

    public string getSmallTalk()
    {
        float ran = Random.Range(0f, 1f);

        if (ran < 0.5f)
            return "Drink cactus juice, it's the quenchiest!";
        else
            return "Swooping is bad!";
    }

    public string[] getConvo()
    {
        string[] convo = { "Hello there", "Hey", "Nice weather today, eh?", "Yes I would say so" };
        return convo;
    }

}
