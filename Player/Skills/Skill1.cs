using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : SkillBase
{
    private Animator animator = PlayerController.instance.animator;
    private void Start()
    {
        skillName = "Skill1";
        icon = Resources.Load<Sprite>("skill1icon");
    }

    public override void Skill()
    {
        animator.Play(skillName);
    }
}
