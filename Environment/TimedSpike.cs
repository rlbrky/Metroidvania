using UnityEngine;

public class TimedSpike : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float spikeForce;
    [SerializeField] [Tooltip("This determines how long the force will be applied.")] private float throwTime = 0.3f;
    [SerializeField] [Tooltip("Lower values will result in higher damage.")] private int scaleAmount = 2;
    [SerializeField] private bool scaleWPlayerHealth;

    PlayerController player;
    Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.instance._spikeTrapSetup.AddListener(AddPlayerScript);
    }

    private void Update()
    {
        m_Animator.Play("TrapActive");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && player)
        {
            player.ThrowPlayer(transform.up, spikeForce, throwTime);
            //GameManager.instance.Player.GetComponent<PlayerController>().ThrowPlayer();
            if(scaleWPlayerHealth)
            {
                GameManager.instance._playerHealth.DamageUnit(GameManager.instance._playerHealth.MaxHealth / scaleAmount);
            }
            else
                GameManager.instance._playerHealth.DamageUnit(damage);
        }
    }

    void AddPlayerScript()
    {
        player = GameManager.instance.Player.GetComponent<PlayerController>();
    }
}
