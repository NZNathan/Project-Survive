using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability {

    void setTarget(CMoveCombatable caster, Vector2 pos);

    void setCooldown(bool cooldown); 

    bool canComboAttack();

    bool onCooldown();

    float getCooldown();

    float getAbilityVelocity();

    string getAnimation();

    string getName();

    IEnumerator getAction();
}
