using Unity.Netcode;
using UnityEngine;

public class TheCube : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _autre; 

    void Start()
    {
       

      

    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("the cube is spawn"); 
        if (IsServer)
        {
            // This won't send any network packets but will log it locally on the server
            NetworkLog.LogInfoServer("Hello World!");
        }

        if (IsClient)
        {
            // This will log locally and send the log to the server to be logged there aswell
            NetworkLog.LogInfoServer("Hello World!");
        }
    }

        // Update is called once per frame
        void Update()
    {

    }

    private void OnConnectedToServer()
    {
        Debug.Log("i'm connected to a server");
    }

  
}
