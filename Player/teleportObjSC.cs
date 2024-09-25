using UnityEngine;

public class teleportObjSC : MonoBehaviour
{
    [SerializeField] private float cannonForceDecreaseRate;
    [SerializeField] private float throwForce;

    public float canStayCountdown = 2f;
    public bool isPlayerInside;

    private void Update()
    {
        if(isPlayerInside && Input.GetMouseButtonDown(0))
        {
            Throw();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.canEnter = true;
            PlayerController.instance._tpSC = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.canEnter = false;
            PlayerController.instance._tpSC = null;
        }
    }

    private void Throw()
    {
        Vector3 mouseCoords = Camera.main.ScreenPointToRay(Input.mousePosition).direction;

        PlayerController.instance.Cannonball(new Vector3(0, mouseCoords.y, mouseCoords.z), throwForce, cannonForceDecreaseRate);
        PlayerController.instance.canEnter = false;
        isPlayerInside = false;
    }

    public void CancelCannon()
    {
        isPlayerInside = false;
    }
}
