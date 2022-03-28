using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserMenu : MonoBehaviour
{
    public static bool IsShown;
    public static string ButtonText;
    public GameObject UserMenuContent;
    public TMP_Text buttonText;

    private static bool prevState;

    private void Start()
    {
        IsShown  = true;
        prevState = !IsShown;
        ButtonText = "Start";
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


}
