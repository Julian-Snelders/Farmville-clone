using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public float wood;
    public float stones;
    public float food;

    [Header("UI Reference")]
    public Text resourcesText;

  
    void FixedUpdate()
    {
        resourcesText.text = "Wood: " + wood.ToString("f0") + " | Stones: "
            + stones.ToString("f0") + " | Food: " + food.ToString("f0");
    }
}
