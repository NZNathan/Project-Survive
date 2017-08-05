using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {

    public static HealthBarManager instance;

    public HealthBar baseHealthBar;

    private void Start()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    public HealthBar newHealthBar()
    {
        HealthBar hpBar = Instantiate(baseHealthBar, transform);

        return hpBar;
    }
}
