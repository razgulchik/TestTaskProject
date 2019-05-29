using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesAmountDisplay : MonoBehaviour
{
    Text livesAmountText;

    // Start is called before the first frame update
    void Start()
    {
        livesAmountText = GetComponent<Text>();
    }

    public void UpdateLivesAmount(int livesAmount)
    {
        livesAmountText.text = livesAmount.ToString();
    }

}
