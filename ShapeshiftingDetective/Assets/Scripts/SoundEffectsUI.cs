using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsUI : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioClip onSelect;
    [SerializeField] private AudioClip onClick;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayOnSelect()
    {
        _audioSource.PlayOneShot(onSelect);
    }

    public void PlayOnClick()
    {
        _audioSource.PlayOneShot(onClick);
    }
}
