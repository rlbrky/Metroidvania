using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grassInteractiveSC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerObj;
    [SerializeField] Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_Player", playerObj.transform.position+offset);
    }
}
