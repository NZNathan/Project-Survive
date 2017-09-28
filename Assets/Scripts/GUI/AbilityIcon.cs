using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour {

    public Image icon;
    public Image cooldown;

    private Ability ability;
    private float animationPause = 0.05f; //Time between calls in the cooldown animation coroutine

	public void setAbility(Ability ability)
    {
        //Reset the cooldown if the ability haad been used and then the player died
        if (ability != null)
        {
            StopAllCoroutines();
            ability.setCooldown(false);
            cooldown.gameObject.SetActive(false);
        }
        icon.sprite = ability.getIcon();
        //abilityName.text = ability.getName();

        this.ability = ability;
    }

    public void startCooldown()
    {
        ability.setCooldown(true);
        cooldown.fillAmount = 1;
        cooldown.gameObject.SetActive(true);
        StartCoroutine("cooldownAnimation");
    }

    /// <summary>
    /// If a cooldown has been stopped when not done, restart it
    /// </summary>
    public void restartCooldown()
    {
        if (cooldown.fillAmount > 0)
            StartCoroutine("cooldownAnimation");
    }
	
	IEnumerator cooldownAnimation()
    {
        while (cooldown.fillAmount > 0) {

            cooldown.fillAmount -= 1 / (ability.getCooldown() / animationPause);

            yield return new WaitForSeconds(animationPause);
        }

        ability.setCooldown(false);
        cooldown.gameObject.SetActive(false);
    }
}
