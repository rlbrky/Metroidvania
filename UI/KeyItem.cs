using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyItem : MonoBehaviour
{

    public Sprite itemIcon;
    public string itemName;
    public string whereItBelongs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.canGrabKeyItem = true;
            PlayerController.instance.currentItem = this;
            UIManager.instance._keyPrompt.gameObject.GetComponent<FollowGameObject>().Obj = gameObject;
            UIManager.instance._keyPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController.instance.canGrabKeyItem = false;
        PlayerController.instance.currentItem = null;
        UIManager.instance._keyPrompt.gameObject.GetComponent<FollowGameObject>().Obj = null;
        UIManager.instance._keyPrompt.gameObject.SetActive(false);
    }

    //This will be called from where the key item will be used.
    public void OnUse()
    {

    }
}
