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

    
    public List<ClipTransition> ClipsAnimancer;
   
    [System.Serializable]
    public struct SerializedClipData
    {
        public string clipName;
        // Ajoutez d'autres informations sur le clip si nécessaire
    }

    // public ClipToSend Clips;
 

    private void Start()
    {
        
        _animancer = GetComponent<AnimancerComponent>();
    
    }


    // Start is called before the first frame update
    [ClientRpc]
    public void SendAnimancerStateClientRpc(string clipName)
    {
        Debug.Log("i've received a message from the server");
        //abstract this call guardBehavior or NetworkPlayerController
        ClipTransition goodClip = null; 
        foreach(ClipTransition c in ClipsAnimancer)
        {
            if(c.Clip.name == clipName)
            {
                goodClip = c; 
            }

        }
        //var c = ClipsAnimancer.Find(clip => clip.Name == clipName);
        _animancer.Play(goodClip); 

    }

   
}
