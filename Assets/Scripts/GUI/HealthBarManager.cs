using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {

    public Transform healthBarParent;

    public HealthBar baseHealthBar;

    public HealthBar newHealthBar()
    {
        HealthBar hpBar = Instantiate(baseHealthBar, healthBarParent);

        return hpBar;
    }
}
