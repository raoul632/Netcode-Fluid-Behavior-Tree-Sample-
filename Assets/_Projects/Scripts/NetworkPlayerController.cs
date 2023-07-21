using Animancer;
using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Playables;

public class NetworkPlayerController : NetworkBehaviour
{
    private CharacterController _characterController;
    private Transform _spawningPoint;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed; 

    [SerializeField]
    public AnimancerComponent _animancer;

    NetworkAnimancer _networkAnimancer;

    // client caches positions
    private Vector3 oldInputPosition = Vector3.zero;
    private Vector3 oldInputRotation = Vector3.zero;

    [SerializeField]
    private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();

    [SerializeField]
    private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animancer = GetComponent<AnimancerComponent>();
        _networkAnimancer = GetComponent<NetworkAnimancer>();




    }

    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();

        //pick the prefab PopClient and place the spider with the transform position 
        if (IsOwner && IsClient)
        {
            GameObject go = null;
            foreach (var elemInPrefabList in NetworkManager.Singleton.NetworkConfig.Prefabs.NetworkPrefabsLists[0].PrefabList)
            {

                Debug.Log(elemInPrefabList.Prefab.name);

                if (elemInPrefabList.Prefab.name == "PopPlayer")
                {
                    go = elemInPrefabList.Prefab;
                    break;
                }


            }
            transform.position = go.transform.position;
        }
            //gameObject.GetComponent<ClientNetworkTransform>().transform.position = go.transform.position;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMoveRotate();
        ClientVisuals(); 

    }

    private void ClientInput()
    {

        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);


        // forward & backward direction
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputPosition = direction * forwardInput;

        if (forwardInput == 0)
        {
            //animation idle
            //UpdateAnimancerStateServerRpc
        }
        else if (forwardInput > 0 && forwardInput < 0) { 

            //animation walk 
        }

        if (oldInputPosition != inputPosition ||
           oldInputRotation != inputRotation)
        {
            oldInputPosition = inputPosition;
            UpdateClientPositionAndRotationServerRpc(inputPosition * _speed, inputRotation * _rotationSpeed);
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    _characterController.Move(Vector3.forward * _speed);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    _characterController.Move(-Vector3.forward * _speed);
        //}
    }

    [ServerRpc]
    public void UpdateAnimancerStateServerRpc(string clipName)
    {
        
        _networkAnimancer.SendAnimancerStateClientRpc(clipName);
    }


    //in this exemple the client are authoritative to move 
    [ServerRpc]
    public void UpdateClientPositionAndRotationServerRpc(Vector3 newPosition, Vector3 newRotation)
    {
        networkPositionDirection.Value = newPosition;
        networkRotationDirection.Value = newRotation;
    }

    private void ClientMoveRotate()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            _characterController.SimpleMove(networkPositionDirection.Value);
        }
        if (networkRotationDirection.Value != Vector3.zero)
        {
            transform.Rotate(networkRotationDirection.Value, Space.World);
        }
    }

    private void ClientVisuals()
    {

    }
}
