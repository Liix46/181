using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceIndicator : MonoBehaviour
{
    public static float ForceFactor;
    private Image image;

    void Start()
    {
        ForceFactor = 0f;
        image = GetComponent<Image>();
        image.fillAmount = ForceFactor;
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        ForceFactor += v * Time.deltaTime;
        //if (ForceFactor < 0.1) ForceFactor = 0.1f;
        if (ForceFactor > 1) ForceFactor = 1f;

        image.fillAmount = ForceFactor;
    }
}
