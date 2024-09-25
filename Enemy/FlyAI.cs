using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyAI : MonoBehaviour
{
    [SerializeField] Transform realEnemy;
    // will be setted from outside
    [SerializeField] float speed = 15f;
    [SerializeField] float checkDistance = 2f;
    [SerializeField] float startCheckDistance = 2f;
    [SerializeField] float turnSpeed = 4f;
    [SerializeField] float bounceMultiplier = 1.5f;
    [SerializeField] float stopDistance = 10f;
    [SerializeField] float stopSpeed = 6f;

    Transform target;
    RaycastHit hit;
    Vector3 dir;
    Vector3 lastDir;
    Vector3 ashasasa;
    Collider _collider;

    bool isActive;

    bool stopped;
    // Start is called before the first frame update
    void Start()
    {
        stopped = false;
        dir = Vector3.zero;
        ashasasa = Vector3.zero;
        _collider = gameObject.GetComponent<Collider>();

        foreach (var playerCollider in GameManager.instance.Player.GetComponentsInChildren<Collider>())
        {
            if(playerCollider.name != "SwordSimulation")
                Physics.IgnoreCollision(playerCollider, _collider);
        }

        StartCoroutine(GetPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
            ChasePlayerUpdate();
    }

    private void LateUpdate()
    {
        if (isActive)
            ChasePlayerLateUpdate();

    }

    
    void ChasePlayerUpdate()
    {
        dir = target.position - transform.position;
        dir = dir.normalized;

        CheckArea();
    }

    void ChasePlayerLateUpdate()
    {
        dir = dir.normalized;

        if (!stopped)
        {

            if (stopDistance > (transform.position - target.position).magnitude)
            {
                stopped = true;
                return;
            }

            transform.position = Vector3.Lerp(transform.position, transform.position + dir, speed * Time.deltaTime);
            lastDir = dir;
        }
        else
        {
            dir = Vector3.Lerp(lastDir, Vector3.zero, stopSpeed * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, transform.position + dir, speed * Time.deltaTime);
            
            lastDir = dir;
            if (stopDistance <= (transform.position - target.position).magnitude)
            {

                stopped = false;
                return;
            }
    }


        if(transform.position.z - target.position.z < 0f)
        {
            realEnemy.transform.rotation = Quaternion.Euler(0,360f,0);
        }
        else
        {
            realEnemy.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        
        Debug.DrawRay(transform.position, dir * 3f, Color.yellow);

    }

    void CheckArea()
    {
        if (stopped) return;

        ashasasa = Vector3.zero;

        Debug.DrawRay(transform.position + Vector3.forward * startCheckDistance, Vector3.forward * checkDistance, Color.cyan);
        Debug.DrawRay(transform.position + Vector3.back * startCheckDistance, -Vector3.forward * checkDistance, Color.cyan);
        Debug.DrawRay(transform.position + Vector3.up * startCheckDistance, Vector3.up * checkDistance, Color.cyan);
        Debug.DrawRay(transform.position + Vector3.down * startCheckDistance, -Vector3.up * checkDistance , Color.cyan);

        if (Physics.Raycast(transform.position + Vector3.forward * startCheckDistance, Vector3.forward, out hit, checkDistance))
        {
            ashasasa += -Vector3.forward;
            ashasasa.y += Mathf.Clamp(target.transform.position.y - transform.position.y,-1f,1f);
        }
        if (Physics.Raycast(transform.position + Vector3.back * startCheckDistance,  - Vector3.forward, out hit, checkDistance))
        {
            ashasasa += Vector3.forward;
            ashasasa.y += Mathf.Clamp(target.transform.position.y - transform.position.y,-1f,1f);
        }

        if (Physics.Raycast(transform.position + Vector3.up * startCheckDistance, Vector3.up, out hit, checkDistance))
        {
            if (hit.transform.gameObject.tag != "Ground")
            {
                var component = hit.transform.GetComponent<MeshFilter>();
                if (component == null)
                    return;
                var widthDiv2 = component.mesh.bounds.extents.z * hit.transform.lossyScale.z;

                float leftX = hit.transform.position.z - widthDiv2;
                float forward = hit.transform.position.z + widthDiv2;

                
                if(Mathf.Abs(target.position.z - transform.position.z)> Mathf.Abs(hit.point.z - leftX))
                {
                    // go left
                    ashasasa -= Vector3.forward;
                }
                else if (Mathf.Abs(target.position.z - transform.position.z) > Mathf.Abs(hit.point.z - forward))
                {
                    // go forward
                    ashasasa += Vector3.forward ;
                }
                else
                {
                    if (Mathf.Abs(hit.point.z - leftX) > Mathf.Abs(hit.point.z - forward))
                    {
                        // forwarda yakýn
                        ashasasa += Vector3.forward;

                    }
                    else
                    {
                        ashasasa -= Vector3.forward;
                    }
                }

            }
            ashasasa += -Vector3.up ;

        }
        if(Physics.Raycast(transform.position + Vector3.down * startCheckDistance,-Vector3.up, out hit , checkDistance))
        {
            if(hit.transform.gameObject.tag != "Ground")
            {
                var component = hit.transform.GetComponent<MeshFilter>();
                if (component == null)
                    return;
                var widthDiv2 = component.mesh.bounds.extents.z * hit.transform.lossyScale.z;

                float leftX = hit.transform.position.z - widthDiv2;
                float forward = hit.transform.position.z + widthDiv2;
                

                if(Mathf.Abs(target.position.z - transform.position.z)> Mathf.Abs(hit.point.z - leftX))
                {
                    // go left
                    ashasasa -= Vector3.forward ;
                }
                else if (Mathf.Abs(target.position.z - transform.position.z) > Mathf.Abs(hit.point.z - forward))
                {
                    // go forward
                    ashasasa += Vector3.forward;
                }
                else
                {
                    if (Mathf.Abs(hit.point.z - leftX) > Mathf.Abs(hit.point.z - forward))
                    {
                        // forwarda yakýn
                        ashasasa += Vector3.forward;


                    }
                    else
                    {
                        ashasasa -= Vector3.forward ;
                    }
                }

            }
            ashasasa += Vector3.up;

            
        }

        dir = Vector3.Lerp(lastDir, dir + ashasasa.normalized * bounceMultiplier, 0.3f * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);

        dir = dir + ashasasa.normalized * bounceMultiplier;
    }


    IEnumerator GetPlayer()
    {
        yield return new WaitForSeconds(1);
        target = GameManager.instance.Player.transform;
        lastDir = target.position - transform.position;
        lastDir = lastDir.normalized;
        isActive = true;
    }
}
