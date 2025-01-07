using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    private static int finalCompanyRevenue;
    private static int finalUserCount;
    private static float timeWhenGameStarted;
    private static float timeAlive;

    // Returns the final score (company revenue) at the end of the game.
    public static int GetFinalScore()
    {
        return finalCompanyRevenue;
    }

    // Returns the number of word spawners (users) in game at the end of the game.
    public static int GetFinalUserCount()
    {
        return finalUserCount;
    }

    // Returns how long the player spent alive.
    public static float GetFinalTimeLasted()
    {
        return timeAlive;
    }

    // Saves any data that needs to be transferred to the win or death screens.
    private static void NoteDownScoreData()
    {
        finalCompanyRevenue = ScoreManager.GetScore();
        finalUserCount = ScoreManager.GetNumUsers();
        timeAlive = Time.time - timeWhenGameStarted;
    }

    // Switches to the death screen when the player dies (i.e. gets fired for not doing well enough).
    public static void PlayerDied()
    {
        NoteDownScoreData();
        SceneManager.LoadScene("GameOver");
    }

    // Switches to the company win screen when the company wins (i.e. makes $10,000).
    public static void CompanyWon()
    {
        NoteDownScoreData();
        SceneManager.LoadScene("CompanyWon");
    }

    // Saves a timestamp of when this function is called. NOTE: this function should be called at the very beginning
    // when a new game gets started, so that timekeeping is accurate.
    public static void InitializeGameStartedTime()
    {
        timeWhenGameStarted = Time.time;
    }
}
