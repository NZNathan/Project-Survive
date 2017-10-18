using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : Interactable
{
    [TextArea]
    public string tip;

    public override void use()
    {
        UIManager.instance.newPopup(tip);
        withinRange = false;
        UIManager.instance.closePopup();
    }
}
