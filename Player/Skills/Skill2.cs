using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : SkillBase
{
    private Animator animator = PlayerController.instance.animator;

    private void Start()
    {
        skillName = "Skill2";
        icon = Resources.Load<Sprite>("skill2icon");

    }
    public override void Skill()
    {
        animator.Play(skillName);
    }
}
