using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class clickCounterModifier : MonoBehaviour
{
    public int clicks;
    public TMP_Text counter;

    // Start is called before the first frame update
    void Start()
    {
        counter.text = "Clicks: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicks++;
            counter.text = "Clicks: " + clicks;
        }
    }
}
