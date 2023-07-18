using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Transform _guardNetworkPrefab;


   
   
    private void Awake()
    {
        

    }


    void Start()
    {
        DontDestroyOnLoad(this); 
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Debug.Log("On Network Spawn ");
           // NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneLoaded;
            Transform spawnedGuard = Instantiate(_guardNetworkPrefab);
            spawnedGuard.GetComponent<NetworkObject>().Spawn(true);
        }
    }


    private void SceneLoaded(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimeOut)
    {

        Debug.Log("On Network Scene Loaded ");
        if (IsServer )
        {
            Debug.Log("On Network Scene Loaded IsServer");
            Transform spawnedGuard = Instantiate(_guardNetworkPrefab);
            spawnedGuard.GetComponent<NetworkObject>().Spawn(true);
        }
    }

}
