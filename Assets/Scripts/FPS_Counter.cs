using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Counter : MonoBehaviour
{
    private TextMeshProUGUI textDisplay;
    int fps;

    private void Start()
    {
        textDisplay = this.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        textDisplay.text = ((int)(1f / Time.deltaTime)).ToString() + "fps";
    }
}
