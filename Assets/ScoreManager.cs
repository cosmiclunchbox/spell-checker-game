using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    private int score = 0;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        CheckInstantiated();
        instance.score += 1;
    }

    // Returns the player's current score.
    public static int GetScore()
    {
        CheckInstantiated();
        return instance.score;
    }
}
