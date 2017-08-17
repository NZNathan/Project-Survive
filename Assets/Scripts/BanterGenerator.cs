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
            return "Take this!";
        else
            return "Why won't you die?!";
    }

}
