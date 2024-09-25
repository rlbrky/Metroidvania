using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillTreeUI : MonoBehaviour
{
    public void OnSkill1Add()
    {
        if (PlayerController.instance.gameObject.GetComponent<Skill1>() == null)
            PlayerController.instance.gameObject.AddComponent<Skill1>();
        StartCoroutine(ChangedSkills());
    }

    public void OnSkill2Add()
    {
        if (PlayerController.instance.gameObject.GetComponent<Skill2>() == null)
            PlayerController.instance.gameObject.AddComponent<Skill2>();
        StartCoroutine(ChangedSkills());
    }

    public void OnSkill3Add()
    {
        if(PlayerController.instance.gameObject.GetComponent<Skill3>() == null)
            PlayerController.instance.gameObject.AddComponent<Skill3>();
        StartCoroutine(ChangedSkills());
    }

    public void ClearSkills()
    {
        foreach(SkillBase skill in PlayerController.instance._skills)
            Destroy(skill);

        PlayerController.instance._skills.Clear();
    }

    IEnumerator ChangedSkills()
    {
        PlayerController.instance.skillsChanged = true;
        yield return new WaitForSeconds(0.1f);
        PlayerController.instance.skillsChanged = false;
    }
}
