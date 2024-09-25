using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float spikeForce;
    [SerializeField] [Tooltip("This determines how long the force will be applied.")] private float throwTime = 0.3f;
    [SerializeField] [Tooltip("Lower values will result in higher damage.")] private int scaleAmount = 2;
    [SerializeField] private bool scaleWPlayerHealth;

    PlayerController player;

    private void Start()
    {
        GameManager.instance._spikeTrapSetup.AddListener(AddPlayerScript);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.ThrowPlayer(gameObject.transform.up, spikeForce, throwTime);

            if (scaleWPlayerHealth)
                GameManager.instance._playerHealth.DamageUnit(GameManager.instance._playerHealth.MaxHealth / scaleAmount);
            else
                GameManager.instance._playerHealth.DamageUnit(damage);
        }
    }

    void AddPlayerScript()
    {
        player = GameManager.instance.Player.GetComponent<PlayerController>();
    }
}
