using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UserMenu : MonoBehaviour
{
    private const string FILE_BEST_STAT = "best_stat.json";

    public static bool IsShown;
    public static string ButtonText;
    public static UserMenuMode userMenuMode;

    public GameObject UserMenuContent;
    public TMP_Text buttonText;
    public TMP_Text statText;
    public TMP_Text bestStatText;
    public Slider SoundSlider;

    public float SoundSliderValue { get; set; }

    private static bool prevState;
    private AudioSource[] _sounds;

    private void Start()
    {
        IsShown  = true;
        prevState = !IsShown;
        ButtonText = "Start";

        _sounds = GetComponents<AudioSource>();
        SoundSliderChange();

        if (File.Exists(FILE_BEST_STAT))
        {

        }
        else
        {
            bestStatText.text = "No best stat";
        }
        statText.text = string.Empty;
    }

    private void Update()
    {
        if (IsShown != prevState) // switch detected
        {
            if (IsShown)
            {
                buttonText.text = ButtonText;
            }

            UserMenuContent.SetActive(IsShown);
            Time.timeScale = IsShown ? 0.0f : 1.0f;

            prevState = IsShown;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsShown = !IsShown;
            if (IsShown) ButtonText = "Continue";
        }
    }

    public void ButtonClick()
    {
        IsShown = false;
        Time.timeScale = 1.0f;
    }

    public static void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    public static bool IsTimeStop()
    {
        if (Time.timeScale == 0.0f)
        {
            return true;
        }
        return false;
    }

    public void SoundSliderChange()
    {
        SoundSliderValue = SoundSlider.value;

        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].volume = SoundSliderValue;
        }

        Debug.Log($"SoundSliderValue {SoundSliderValue}");
    }

}

public enum UserMenuMode
{
    START,
    PAUSE,
    GAMEOVER
}
