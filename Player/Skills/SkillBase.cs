using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Skill Info")]
    public string skillName;
    public Sprite icon;
    public float cooldownTime = 1f;
    
    private bool canUse = true;

    public void UseSkill()
    {
        if(canUse)
        {
            //Can make an event here for UI things.

            Skill();
            StartCooldown();
        }
    }

    public abstract void Skill();

    private void StartCooldown()
    {
        StartCoroutine(Cooldown());
        IEnumerator Cooldown()
        {
            canUse = false;
            yield return new WaitForSeconds(cooldownTime);
            canUse = true;
        }
    }
}
