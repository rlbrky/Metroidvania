using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    [Tooltip("The distance for the platform to cover for both sides.")]
    float distance = 10;

    Vector3 startPos;

    [SerializeField]
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0)
        {
            if (Vector3.Distance(transform.position, startPos) >= distance)
                speed *= -1;
        }
        if (speed < 0)
        {

            if (Vector3.Distance(transform.position, startPos) <= 0.1f)
                speed *= -1;
        }

        transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        #region Can't Get Player Collider BUG
        //This code is here because there was an error that the trigger couldn't find the player,
        //to guarantee that we get the player I've added the others. There should be a better solution but for now the problem isn't there so we are safe atm.
        //if (other.gameObject == player || other.gameObject.name == "GroundCheck" || other.gameObject.name == "WallCheck")
        //    player.transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
        #endregion

        Debug.Log("Platform collided with: " + other.name);

        if (other.gameObject == player)
            player.transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
    }
}