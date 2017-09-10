using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability {

    void setTarget(CMoveCombatable caster, Vector2 pos);

    void setCooldown(bool cooldown);

    /// <summary>
    /// Returns either itself, or the ability it combos into if this ability is activated while comboing is possible
    /// </summary>
    Ability getComboAttack();

    bool canComboAttack();

    bool onCooldown();

    float getCooldown();

    float getAbilityVelocity();

    string getAnimation();

    string getName();

    IEnumerator getAction();
}
