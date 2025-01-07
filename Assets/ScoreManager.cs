using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreDisplay;

    [SerializeField]
    private TextMeshProUGUI numUserDisplay;

    private static ScoreManager instance;
    private int score = 0;
    private int numUsers = 0;

    void Awake()
    {
        // ensures there is only one score manager instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SwitchScenes.InitializeGameStartedTime();
        StartCoroutine(EarnMoney());
    }

    // Update is called once per frame
    void Update()
    {
        // detects whether the company has lost too much money and fires the player if so
        if (score < -250)
        {
            SwitchScenes.PlayerDied();
        }

        // detects whether the company has made a lot of money and triggers company win condition if so
        if (score >= 10000)
        {
            SwitchScenes.CompanyWon();
        }
    }

    // Passively generate money for the CEO's pockets.
    private IEnumerator EarnMoney()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            IncrementScore();
        }
    }

    // Updates the score display to show the current score.
    private void UpdateScoreDisplay()
    {
        if (score < 0)
        {
            scoreDisplay.color = Color.red;
            scoreDisplay.text = "!!!!! company revenue: $" + score;
        }
        else
        {
            scoreDisplay.color = Color.black;
            scoreDisplay.text = "company revenue: $" + score;
        }
    }

    // Updates the user count display to show the current number of users.

    private void UpdateNumUserDisplay()
    {
        numUserDisplay.text = "users: " + numUsers;
    }

    // Checks that the score manager has been properly instantiated before it is used.
    private static void CheckInstantiated()
    {
        if (instance == null)
        {
            throw new System.Exception("Score manager has not been instantiated.");
        }
    }

    // Increases the player's score by 1.
    public static void IncrementScore()
    {
        IncreaseScore(1);
    }

    // Increases the player's score by the given amount.
    public static void IncreaseScore(int amount)
    {
        CheckInstantiated();
        instance.score += amount;
        instance.UpdateScoreDisplay();
    }

    // Returns the player's current score.
    public static int GetScore()
    {
        CheckInstantiated();
        return instance.score;
    }

    // Increases the number of word spawners (users) by 1.
    public static void IncrementNumUsers()
    {
        CheckInstantiated();
        instance.numUsers += 1;
        instance.UpdateNumUserDisplay();
    }

    // Returns the number of word spawners (users) currently in the game.
    public static int GetNumUsers()
    {
        CheckInstantiated();
        return instance.numUsers;
    }
}
