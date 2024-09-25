using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    public GameObject Obj;
    Camera mCamera;
    private RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Obj)
            rt.position = mCamera.WorldToScreenPoint(Obj.transform.position) + offset;
    }
}
