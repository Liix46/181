using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UserMenu : MonoBehaviour
{
    private const string FILE_BEST_STAT = "best_stat.json";

    public static bool IsShown;
    public static string ButtonText;

    public static UserMenuMode userMenuMode;

    private static bool prevState;

    public GameObject UserMenuContent;

    public TMP_Text buttonText;
    public TMP_Text statText;
    public TMP_Text bestStatText;

    public Slider SoundSlider;

    public float SoundSliderValue { get; set; }

    private AudioSource[] _sounds;

    string json = "";

    private  void Start()
    {
        IsShown  = true;
        prevState = !IsShown;
        ButtonText = "Start";

        _sounds = GetComponents<AudioSource>();
        SoundSliderChange();

        userMenuMode = UserMenuMode.START;

        if (File.Exists(FILE_BEST_STAT))
        {
            // Deserialize best stat
            try
            {
                using StreamReader reader = new(FILE_BEST_STAT);
                json = reader.ReadToEnd();
                //JsonUtility.FromJsonOverwrite(json, GameStat.BestMoves);
                GameStat.BestMoves = JsonConvert.DeserializeObject<List<MoveData>>(json);

                Debug.Log($"json {json}");
               

            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            //bestStatText.text = "Yes best stat";
        }
        ShowBestStat();
        //statText.text = string.Empty;
        GameStat.Moves = new();
        ShowCurrentStat();

    }

    private void ShowBestStat()
    {
        
        if (GameStat.BestMoves == null)
        {
            Debug.Log($"Best Moves is NULL");
            bestStatText.text = "Best statistic";
            return;
        }
        string str = "   Best statistic:";
        str += "\n   #\tK\tT\tS";
        foreach (MoveData move in GameStat.BestMoves)
        {
            str += $"\n   {move.Num}\t{move.KegelsFall}\t{move.Time}\t{move.Score}";
        }
        bestStatText.text = str;
    }

    private void ShowCurrentStat()
    {
        if (GameStat.Moves.Count == 0)
        {
            Debug.Log($"Moves is empty");
            statText.text = "Currency statistic:";
            return;
        }
        string str = "Currency statistic:";
        str += "\n   #\tK\tT\tS";
        foreach (MoveData move in GameStat.Moves)
        {
            str += $"\n   {move.Num}\t{move.KegelsFall}\t{move.Time}\t{move.Score}";
        }
        statText.text = str;
    }

    private void Update()
    {
        if (IsShown != prevState) // switch detected
        {
            if (IsShown)
            {
                switch (userMenuMode)
                {
                    case UserMenuMode.START:
                        ButtonText = "Start";
                        break;
                    case UserMenuMode.PAUSE:
                        ButtonText = "Continue";
                        ShowCurrentStat();
                        break;
                    case UserMenuMode.GAMEOVER:
                        ButtonText = "Play again";
                        ShowCurrentStat();
                        SaveStat();
                        break;
                    default:
                        break;
                }



                buttonText.text = ButtonText;
            }

            UserMenuContent.SetActive(IsShown);
            Time.timeScale = IsShown ? 0.0f : 1.0f;

            prevState = IsShown;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            userMenuMode = UserMenuMode.PAUSE;
            IsShown = !IsShown;
        }
    }



    private void SaveStat()
    {
        // Serialize GameStat
        try
        {
            using StreamWriter sw = new(FILE_BEST_STAT);

            List<MoveData> moves = new();
            if (GameStat.BestMoves != null)
            {
                moves.AddRange(GameStat.BestMoves);
            }
            
            moves.AddRange(GameStat.Moves);
            moves = moves.OrderByDescending(x => x.Score).ToList();
            if (moves.Count <=3)
            {
                json = JsonConvert.SerializeObject(moves);
            }
            else
            {
                GameStat.BestMoves = new()
                {
                    moves[0],
                    moves[1],
                    moves[2]
                };
                json = JsonConvert.SerializeObject(GameStat.BestMoves);
            }

            sw.Write(json);
            Debug.Log($"json {json}");

        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public void ButtonClick()
    {
        if (userMenuMode == UserMenuMode.GAMEOVER)
        {
            // restart game (scene)
            SceneManager.LoadScene(0); // build index
        }
        else
        {
            IsShown = false;
            Time.timeScale = 1.0f;
        }
        
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

