using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpAmountDisplay : MonoBehaviour
{
    Text expAmountText;

    // Start is called before the first frame update
    void Start()
    {
        expAmountText = GetComponent<Text>();
        UpdateExpAmount(0);
    }

    public void UpdateExpAmount(int expAmount)
    {
        expAmountText.text = expAmount.ToString();
    }
}
