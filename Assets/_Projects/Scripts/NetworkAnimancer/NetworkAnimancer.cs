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

    public Dictionary<string, ClipTransition> animationDictionary = new Dictionary<string, ClipTransition>();

    // 
    // Add Animation to the dictionnary 
    public void AddAnimationToDictionary(string clipName, ClipTransition animationClip)
    {
        animationDictionary[clipName] = animationClip;
    }

    [System.Serializable]
    public struct SerializedClipData
    {
        public string clipName;
        // Ajoutez d'autres informations sur le clip si nécessaire
    }

    // public ClipToSend Clips;

    private void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
    }

    private void Start()
    {
        
        
    
    }


    // Start is called before the first frame update
    [ClientRpc]
    public void SendAnimancerStateClientRpc(string clipName)
    {
        Debug.Log("i've received a message from the server");
        //abstract this call guardBehavior or NetworkPlayerController

        if (animationDictionary.ContainsKey(clipName))
        {
            ClipTransition clip = null;

            if (animationDictionary.TryGetValue(clipName, out clip))
            {
                //var c = ClipsAnimancer.Find(clip => clip.Name == clipName);
                _animancer.Play(clip);
            }
            else
            {
                Debug.LogWarning($"Animation clip '{clipName}' not found in the dictionary.");
            }
        }

    }

   
}
