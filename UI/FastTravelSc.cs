using System.Collections.Generic;
using UnityEngine;

public class FastTravelSc : MonoBehaviour
{
    [SerializeField] string SceneToGo;
    [SerializeField] string SpawnPointName;
    [SerializeField] string Area;

    public void Teleport()
    {
        UIManager.instance.OnExitUIAltar();

        GameManager.instance._playerHealth.HealUnit(GameManager.instance._playerHealth.MaxHealth);
        GameManager.instance.ChangeScene(SceneToGo, SpawnPointName, Area);
    }
}
