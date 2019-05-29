using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    Text levelText;

    void Start()
    {
        levelText = GetComponent<Text>();
        UpdateLevelText(0);
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = level.ToString();
    }


}
