using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSoundManager : MonoBehaviour
{

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Plays the given sound effect.
    public IEnumerator PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
        yield return null;
    }
}
