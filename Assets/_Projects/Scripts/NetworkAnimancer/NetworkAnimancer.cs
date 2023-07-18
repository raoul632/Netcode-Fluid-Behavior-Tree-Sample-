using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Animancer;
using System.Linq;


//the goal is just to send information to the client 
public class NetworkAnimancer : NetworkBehaviour
{
    AnimancerComponent _animancer;

    [System.Serializable]
    public struct SerializedClipData
    {
        public string clipName;
        // Ajoutez d'autres informations sur le clip si nécessaire
    }

    // public ClipToSend Clips;
    private GuardBehavior _guardBehavior; 

    private void Start()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _guardBehavior = GetComponent<GuardBehavior>(); 
       // Clips = new ClipToSend(); 
    }
    // Start is called before the first frame update
    [ClientRpc]
    public void SendAnimancerStateClientRpc(string clipName)
    {
        Debug.Log("i've received a message from the server");

        var c = _guardBehavior.ClipsAnimancer.Find(clip => clip.Name == clipName);
        _animancer.Play(c); 

    }
}
