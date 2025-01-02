using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WordController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private string word;

    private bool misspelled;
    private float timeBeforeDespawn = 10;
    private bool selected;
    private enum WordDespawnOptions { MISSPELLING_CAUGHT, NORMAL_CAUGHT, MISSPELLING_DESPAWN, NORMAL_DESPAWN };

    // IMPORTANT
    // Naming convention for word list files:
    // [].txt for normal words
    // []_misspelled.txt for misspelled words
    // All word lists must be stored in /Assets/WordLists
    private const string BASE_WORD_LIST_PATH = @"Assets\WordLists\";

    // Start is called before the first frame update
    void Start()
    {
        Initialize("wordlist", 0.2f);
        StartCoroutine(HandleWordLifetime(timeBeforeDespawn));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveWord(0, -30f * Time.fixedDeltaTime);
    }

    // Initializes this word.
    void Initialize(string wordFileName, float misspelledChance)
    {
        displayText.GetComponent<RectTransform>().anchoredPosition = transform.position;
        Debug.Log(transform.position);
        string[] wordList;

        // determine whether the word will be a misspelling or not
        if (Random.Range(0f, 1f) < misspelledChance)
        {
            misspelled = true;
            wordList = File.ReadAllLines(BASE_WORD_LIST_PATH + wordFileName + "_misspelled.txt");
        }
        else
        {
            misspelled = false;
            wordList = File.ReadAllLines(BASE_WORD_LIST_PATH + wordFileName + ".txt");
        }

        // set as a random word from the appropriate word list (depending on whether it's a misspelling)
        AssignWordFromList(wordList);
        SetDisplayWord();

        selected = false;
    }

    // Picks a word from the given list and assigns it.
    private void AssignWordFromList(string[] wordList)
    {
        word = wordList[Random.Range(0, wordList.Length)];
    }

    // Sets the display text to the word.
    private void SetDisplayWord()
    {
        displayText.text = word;
        displayText.color = Color.white;
        displayText.ForceMeshUpdate();

        float displayTextWidth = displayText.textBounds.size.x;
        float displayTextHeight = displayText.GetComponent<RectTransform>().sizeDelta.y;
        displayText.GetComponent<RectTransform>().sizeDelta = new Vector2(displayTextWidth, displayTextHeight);

    }

    // Moves this word by the given amount in the x and y directions.
    private void MoveWord(float xAmount, float yAmount)
    {
        SetPosition(new Vector2(transform.position.x + xAmount, transform.position.y + yAmount));
    }

    // Sets the position of this word to the given 2D point.
    private void SetPosition(Vector2 newPos)
    {
        transform.position = newPos;
        displayText.GetComponent<RectTransform>().anchoredPosition = transform.position;
    }

    // Returns whether the word is misspelled or not, based on what it was initialized as.
    public bool IsMisspelled()
    {
        return misspelled;
    }

    // Called when this word is clicked on by the player. If the word is misspelled, then the player
    // gains a point. Otherwise, the player DIES HORRIFICALLY in the FACE!
    private void WhenWordClicked()
    {
        if (misspelled)
        {
            Debug.Log("Correctly selected misspelled word.");
            ScoreManager.IncrementScore();
            Debug.Log(ScoreManager.GetScore());
            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.MISSPELLING_CAUGHT));
        }
        else
        {
            Debug.Log("HAHA LOOSER");
            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.NORMAL_CAUGHT));
        }
    }

    // Controls how long the word will remain in the game before it despawns.
    private IEnumerator HandleWordLifetime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (misspelled)
        {
            Debug.Log("HAHA LOOSER");
            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.MISSPELLING_DESPAWN));
        }
        else
        {

            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.NORMAL_DESPAWN));
        }

        yield return null;
    }

    // Handles the animation when this word disappears, either by being selected or by despawning.
    private IEnumerator WordDisappearingAnimation(WordDespawnOptions type)
    {
        switch (type)
        {
            case WordDespawnOptions.MISSPELLING_CAUGHT: displayText.color = Color.green; break;
            case WordDespawnOptions.NORMAL_CAUGHT: displayText.color = Color.red; break;
            case WordDespawnOptions.MISSPELLING_DESPAWN: displayText.color = Color.red; break;
            case WordDespawnOptions.NORMAL_DESPAWN: displayText.color = Color.white; break;
            default: displayText.color = Color.white; break;
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
        yield return null;
    }

    // Detects when the player clicks on this word.
    public void OnPointerDown(PointerEventData data)
    {
        if (!selected)
        {
            selected = true;
            WhenWordClicked();
        }
    }
}
