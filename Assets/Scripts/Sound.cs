using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    private AudioSource[] _sounds;
    private AudioSource _backSound;

    public Slider SoundSlider;

    //private UserMenu _menu;


    // Start is called before the first frame update
    void Start()
    {
        //_menu = GameObject.Find("UserMenuCanvas").GetComponent<UserMenu>();
        _sounds = GetComponents<AudioSource>();
        _backSound = _sounds[0];
        _backSound.volume = SoundSlider.value;
        _backSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundSliderChange()
    {
        _backSound.volume = SoundSlider.value;
    }
}
