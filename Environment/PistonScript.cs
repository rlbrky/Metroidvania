using UnityEngine;

public class PistonScript : MonoBehaviour
{
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    [Tooltip("The distance for the piston to cover for both sides.")]
    float distance = 10;

    Vector3 startPos;

    [SerializeField]
    PlayerController player;

    [SerializeField] [Tooltip("Amount of force to apply")] float force = 30f;
    [SerializeField][Tooltip("How long will the force be applied")] float forceTime = 2f;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            player.ThrowPlayer(transform.forward, force, forceTime);
        }
    }
}