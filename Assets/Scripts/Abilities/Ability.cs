using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability {

    void setTarget(CMoveCombatable caster, Vector2 pos, Vector2 direction);

    float getCooldown();

    float getAbilityVelocity();

    string getAnimation();

    void setIEnum(IEnumerator abilityAction);

    IEnumerator getAction();
}
