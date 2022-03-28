using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private float _time;
    private TMP_Text _clock;

    void Start()
    {
        _time = 0;
        _clock = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
    }

    private void LateUpdate()
    {
        int t = (int)_time;
        int h = t / 3600;
        int m = (t % 3600) / 60;
        int s = t % 60;
        int d = (int)((_time - t) * 10);
        _clock.text = (h < 10 ? "0" : "") + h + ":"
            + (m < 10 ? "0" : "") + m + ":"
            + (s < 10 ? "0" : "") + s + "." + d;
    }
}
