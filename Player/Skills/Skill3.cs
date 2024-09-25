using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : SkillBase
{
    private Animator animator = PlayerController.instance.animator;

    private void Start()
    {
        skillName = "Skill3";
        icon = Resources.Load<Sprite>("skill3icon");

    }

    public override void Skill()
    {
        animator.Play(skillName);
    }
}
