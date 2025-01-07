using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalStatsDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI finalScoreDisplay;

    [SerializeField]
    private TextMeshProUGUI finalUserCountDisplay;

    [SerializeField]
    private TextMeshProUGUI finalTimeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        finalScoreDisplay.text = "company revenue: $" + SwitchScenes.GetFinalScore();
        finalUserCountDisplay.text = "number of users: " + SwitchScenes.GetFinalUserCount();
        finalTimeDisplay.text = "employed for: " + (int)SwitchScenes.GetFinalTimeLasted() + " seconds";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
