using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesAmountDisplay : MonoBehaviour
{
    [SerializeField] float livesAmount = 10;
    Text livesAmountText;

    // Start is called before the first frame update
    void Start()
    {
        livesAmountText = GetComponent<Text>();
        UpdateLivesAmount();
    }

    private void UpdateLivesAmount()
    {
        livesAmountText.text = livesAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
