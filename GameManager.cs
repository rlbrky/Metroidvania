using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject _player;
    GameObject _playerCopy;
    public GameObject Player {  get { return _playerCopy; } }
    public static GameManager instance {  get; private set; }

    string _spawnPointName;
    bool _sceneChanged;

    public UnityEvent _spikeTrapSetup = new UnityEvent();
    public UnitHealth _playerHealth = new UnitHealth(100, 100);

    public string areaName;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        _player = (GameObject)Resources.Load("KarakterNew", typeof(GameObject));
        areaName = "Forest";
        //DontDestroyOnLoad(_playerCopy);
    }

    private void Update()
    {
        //Debug.Log("Player has " + _playerHealth.Health + " health left.");
    }

    public void ChangeScene(string sceneName, string spawnPointName, string givenAreaName)
    {
        SceneManager.LoadScene(sceneName);
        _spawnPointName = spawnPointName;
        areaName = givenAreaName;
        _sceneChanged = true;
    }

    public void SpawnPlayer()
    {
        if(_sceneChanged && _player != null)
        {
            //_playerCopy = GameObject.Instantiate(_player, GameObject.Find(_spawnPointName).transform.position, Quaternion.identity);
            _playerCopy.transform.position = GameObject.Find(_spawnPointName).transform.position;
            PlayerController.instance.maara = _playerCopy.transform.position;
            _sceneChanged = false;
            UIManager.instance.ShowKeyItems(areaName);
            _spikeTrapSetup.Invoke();

        }
        else if(_player != null)
        {
            _playerCopy = GameObject.Instantiate(_player, GameObject.Find("SpawnPoint1").transform.position, Quaternion.identity);
            PlayerController.instance.maara = _playerCopy.transform.position;
            UIManager.instance.ShowKeyItems(areaName);
            _spikeTrapSetup.Invoke();
            DontDestroyOnLoad(_playerCopy);
        }
    }
}
