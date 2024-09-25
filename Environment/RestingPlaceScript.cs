using UnityEngine;

public class RestingPlaceScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance._altarSc = this;
            UIManager.instance._keyPrompt.gameObject.GetComponent<FollowGameObject>().Obj = gameObject;
            UIManager.instance._keyPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance._altarSc = null;
            UIManager.instance.OnExitUIAltar();
        }
    }
}
