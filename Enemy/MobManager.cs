using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    [Header("Pool Info")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private int maxMobAmount;

    private Stack<GameObject> pool = new Stack<GameObject>();


    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        if(prefabToSpawn != null)
        {
            for (int i =0; i < maxMobAmount; i++)
            {
                var newObj = Instantiate(prefabToSpawn);
                newObj.SetActive(false);
                pool.Push(newObj);
            }
        }
    }

    public GameObject UseObj_Pool()
    {
        while (pool.Count > 0)
        {
            var obj = pool.Pop();

            if (obj != null)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        Debug.LogError("All pooled objects are already in use.");
        return null;
    }

    public void ReturnObj_Pool(GameObject toReturn)
    {
        if (toReturn != null)
        {
            toReturn.SetActive(false);
            pool.Push(toReturn);
        }
    }
}
