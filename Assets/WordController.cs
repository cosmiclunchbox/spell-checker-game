using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WordController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private string word;

    private bool misspelled;
    private float timeBeforeDespawn = 4;
    private bool selected;
    private bool decaying = false;
    private enum WordDespawnOptions { MISSPELLING_CAUGHT, NORMAL_CAUGHT, MISSPELLING_DESPAWN, NORMAL_DESPAWN };

    private Color wordNormalColor = Color.black;
    private Color wordHoveredColor = Color.gray;
    private Color wordFailColor = Color.red;
    private Color wordSuccessColor = Color.green;

    // IMPORTANT
    // Naming convention for word list files:
    // [].txt for normal words
    // []_misspelled.txt for misspelled words
    // All word lists must be stored in /Assets/WordLists
    // NOTE: this field is no longer used because the method of accessing word lists has been updated
    private const string BASE_WORD_LIST_PATH = @"Assets\WordLists\";

    // NOTE: the lists must be given in the same order as the enum values are defined in WordSpawner.Type
    // i.e. normal, school student, college student, fanfic, author, scientist, typer
    [SerializeField]
    private TextAsset[] wordLists;

    // NOTE: see above note
    [SerializeField]
    private TextAsset[] wordMisspelledLists;

    [SerializeField]
    private AudioClip successSound;

    [SerializeField]
    private AudioClip failureSound;

    [SerializeField]
    private AudioClip missSound;

    private WordSoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleWordLifetime(timeBeforeDespawn));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //MoveWord(0, -30f * Time.fixedDeltaTime);
        //Debug.Log(boxCollider.bounds.center);
        /*boxCollider.enabled = false;
        bool spawnLocationObstructed = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.zero, 0);
        boxCollider.enabled = true;

        if (spawnLocationObstructed)
        {
            SetPosition(new Vector2(Random.Range(-200, 200), Random.Range(-100, 100)));
        }*/
    }

    // Initializes this word. NOTE: This method must be called immediately after a WordController is created.
    // The responsibility for calling this method falls upon the script that creates the WordController.
    // NOTE: this function has been replaced, see below for new function.
    public void Initialize(string wordFileName, float misspelledChance)
    {

        /*displayText.GetComponent<RectTransform>().anchoredPosition = transform.position;
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

        SetWordHitboxSize();
        SpawnWordInGame(new Vector2(-600, 50), new Vector2(600, -400));

        selected = false;

        soundManager = gameObject.GetComponent<WordSoundManager>();*/
        throw new System.NotImplementedException();
    }

    // Initializes this word. NOTE: This method must be called immediately after a WordController is created.
    // The responsibility for calling this method falls upon the script that creates the WordController.
    public void Initialize(WordSpawner.Type wordSpawnerType, float misspelledChance)
    {
        displayText.GetComponent<RectTransform>().anchoredPosition = transform.position;
        string[] wordList;
        int wordListIndex = (int)wordSpawnerType;

        // determine whether the word will be a misspelling or not
        if (Random.Range(0f, 1f) < misspelledChance)
        {
            misspelled = true;
            wordList = wordMisspelledLists[wordListIndex].text.Split('\n');
        }
        else
        {
            misspelled = false;
            wordList = wordLists[wordListIndex].text.Split('\n');
        }

        // set as a random word from the appropriate word list (depending on whether it's a misspelling)
        AssignWordFromList(wordList);
        SetDisplayWord();

        SetWordHitboxSize();
        SpawnWordInGame(new Vector2(-600, 50), new Vector2(600, -400));

        selected = false;

        soundManager = gameObject.GetComponent<WordSoundManager>();
    }

    // Picks a word from the given list and assigns it.
    private void AssignWordFromList(string[] wordList)
    {
        word = wordList[Random.Range(0, wordList.Length)];
    }

    // Adjusts the size of the hitbox of this word to fit the length of the word.
    private void SetWordHitboxSize()
    {
        float hitboxWidth = displayText.textBounds.size.x;
        float hitboxHeight = displayText.GetComponent<RectTransform>().sizeDelta.y;
        boxCollider.size = new Vector2(hitboxWidth, hitboxHeight);
    }

    // Sets the display text to the word.
    private void SetDisplayWord()
    {
        displayText.text = word;
        displayText.color = wordNormalColor;
        displayText.ForceMeshUpdate();

        float displayTextWidth = displayText.textBounds.size.x;
        float displayTextHeight = displayText.GetComponent<RectTransform>().sizeDelta.y;
        displayText.GetComponent<RectTransform>().sizeDelta = new Vector2(displayTextWidth, displayTextHeight);

    }

    // Places this word at a random position within the rectangular region defined by the given points
    // such that it doesn't overlap with any other words in the game.
    private void SpawnWordInGame(Vector2 topLeft, Vector2 bottomRight)
    {
        bool spawnLocationObstructed;
        int spawnAttempts = 0;
        do
        {
            SetPosition(new Vector2(Random.Range(topLeft.x, bottomRight.x), Random.Range(bottomRight.y, topLeft.y)));
            boxCollider.enabled = false;
            spawnLocationObstructed = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.zero, 0);
            boxCollider.enabled = true;
            spawnAttempts += 1;
        }
        while (spawnLocationObstructed && spawnAttempts < 25);

        if (spawnAttempts >= 25)
        {
            Debug.Log("Failed to find valid spawning location for word.");
            Destroy(gameObject);
            //throw new System.Exception("Failed to find valid spawning location for word.");
        }
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
        //GetComponent<Collider>().transform.position = transform.position;
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
            ScoreManager.IncreaseScore(100);
            Debug.Log(ScoreManager.GetScore());
            StartCoroutine(soundManager.PlaySound(successSound));
            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.MISSPELLING_CAUGHT));
        }
        else
        {
            Debug.Log("HAHA LOOSER");
            ScoreManager.IncreaseScore(-100);
            StartCoroutine(soundManager.PlaySound(failureSound));
            StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.NORMAL_CAUGHT));
        }
    }

    // Controls how long the word will remain in the game before it despawns.
    private IEnumerator HandleWordLifetime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (!selected)
        {
            decaying = true;

            if (misspelled)
            {
                Debug.Log("HAHA LOOSER");
                ScoreManager.IncreaseScore(-100);
                StartCoroutine(soundManager.PlaySound(missSound));
                StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.MISSPELLING_DESPAWN));
            }
            else
            {

                StartCoroutine(WordDisappearingAnimation(WordDespawnOptions.NORMAL_DESPAWN));
            }
        }

        yield return null;
    }

    // Handles the animation when this word disappears, either by being selected or by despawning.
    private IEnumerator WordDisappearingAnimation(WordDespawnOptions type)
    {
        switch (type)
        {
            case WordDespawnOptions.MISSPELLING_CAUGHT: displayText.color = wordSuccessColor; break;
            case WordDespawnOptions.NORMAL_CAUGHT: displayText.color = wordFailColor; break;
            case WordDespawnOptions.MISSPELLING_DESPAWN: displayText.color = wordFailColor; break;
            case WordDespawnOptions.NORMAL_DESPAWN: displayText.color = wordNormalColor; break;
            default: displayText.color = wordNormalColor; break;
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

    // Detects when the player mouses over this word.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected && !decaying)
        {
            displayText.color = wordHoveredColor;
        }
    }

    // Detects when the player stops mousing over this word.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected && !decaying)
        {
            displayText.color = wordNormalColor;
        }
    }
}
