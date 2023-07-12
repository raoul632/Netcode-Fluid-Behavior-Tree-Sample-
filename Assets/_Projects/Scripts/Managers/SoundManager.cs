using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    AudioSource _audioSource; 

    private void Awake()
    {
        if(Instance != null && this != Instance)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;
        _audioSource = GetComponent<AudioSource>(); 
    }

    public void PlayAudioClip(AudioClip audioClip, float volume = 1)
    {
        _audioSource.PlayOneShot(audioClip, volume); 
    }


    public void PlayMusic()
    {

    }

    public void StopMusic()
    {
        _audioSource.Stop(); 
    }



}
