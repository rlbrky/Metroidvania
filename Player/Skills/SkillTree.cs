using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public void OnSkill1Add()
    {
        PlayerController.instance.gameObject.AddComponent<Skill1>();
    }

    public void OnSkill2Add()
    {
        PlayerController.instance.gameObject.AddComponent<Skill2>();
    }

    public void OnSkill3Add()
    {
        PlayerController.instance.gameObject.AddComponent<Skill3>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }
}
