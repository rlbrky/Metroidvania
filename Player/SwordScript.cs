using UnityEngine;

public class SwordScript : MonoBehaviour
{
    [SerializeField] [Tooltip("The amount of damage this deals.")] int strength;

    GameObject _enemy;

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Enemy")
    //    {
    //        collision.collider.GetComponent<Animator>().SetBool("getHit", true);
    //        Debug.Log("Enemy damaged");
    //    }
    //}
    public int GetStrength()
    {
        return strength;
    }

    public void SetEnemy()
    {
        _enemy = null;
    }

    public GameObject GetEnemy()
    {
        return _enemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            _enemy = other.gameObject;
            //other.GetComponent<Animator>().SetBool("getHit", true);
        }

    }
}
