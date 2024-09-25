using UnityEngine;

public class stepOffsetScript : MonoBehaviour
{
    public bool groundExists;
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Ground")
            groundExists = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        groundExists = false;
    }
}
