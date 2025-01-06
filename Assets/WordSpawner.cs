using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordSpawner : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI personName;

    [SerializeField]
    private TextMeshProUGUI personDescription;

    [SerializeField]
    private GameObject personImage;

    [SerializeField]
    private GameObject wordPrefab;

    private static readonly string[] POSSIBLE_NAMES = { "mike", "alice", "kevin", "denise", "karen", "albert", "avani", "nathan", "hatsune miku"};

    // IMPORTANT
    // Naming convention for word list files:
    // [].txt for normal words
    // []_misspelled.txt for misspelled words
    // All word lists must be stored in /Assets/WordLists
    private string wordListFileName;
    private float misspellingChance;
    private float wordSpawnInterval;

    // this fields are no longer used, since the word spawner is no longer in charge of
    // positioning the words after they are spawned in
    private Vector2 spawnPos;
    private Queue<Vector2> prevSpawnPos = new Queue<Vector2>();

    public enum Type
    {
        NORMAL,
        SCHOOL,
        COLLEGE,
        FANFIC,
        AUTHOR,
        SCIENTIST,
        TYPER,
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize(0, Type.NORMAL);
        StartCoroutine(SpawnWordsOnInterval(wordSpawnInterval));
    }

    // Initialize this word spawner. NOTE: This method must be called immediately after a WordSpawner is created.
    public void Initialize(int positionIndex, Type type)
    {
        SetPosition(new Vector2(-800 + positionIndex * 250, 250));
        personName.text = POSSIBLE_NAMES[Random.Range(0, POSSIBLE_NAMES.Length)];

        switch (type)
        {
            case Type.NORMAL:
                wordListFileName = "wordlist";
                misspellingChance = 0.25f;
                personDescription.text = "just a person";
                wordSpawnInterval = 2.5f;
                break;
            case Type.SCHOOL:
                wordListFileName = "school_list";
                misspellingChance = 0.4f;
                personDescription.text = "middle schooler";
                wordSpawnInterval = 3f;
                break;
            case Type.COLLEGE:
                wordListFileName = "college_list";
                misspellingChance = 0.25f;
                personDescription.text = "computer science major";
                wordSpawnInterval = 2f;
                break;
            case Type.FANFIC:
                wordListFileName = "fanfic_list";
                misspellingChance = 0.4f;
                personDescription.text = "fanfiction writer";
                wordSpawnInterval = 2f;
                break;
            case Type.AUTHOR:
                wordListFileName = "author_list";
                misspellingChance = 0.1f;
                personDescription.text = "bestselling author";
                wordSpawnInterval = 1f;
                break;
            case Type.SCIENTIST:
                wordListFileName = "scientist_list";
                misspellingChance = 0.15f;
                personDescription.text = "scientist";
                wordSpawnInterval = 1.5f;
                break;
            case Type.TYPER:
                wordListFileName = "typer_list";
                misspellingChance = 0.5f;
                personDescription.text = "professional fast typer";
                wordSpawnInterval = 0.75f;
                break;
            default:
                throw new System.Exception("Unrecognized word spawner type.");
        }
    }

    // Sets the position of this word spawner and its child objects to the given 2D location.
    private void SetPosition(Vector2 pos)
    {
        transform.position = pos;
        personImage.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + new Vector2(0, 140);
        personName.GetComponent<RectTransform>().anchoredPosition = transform.position;
        personDescription.GetComponent<RectTransform>().anchoredPosition = (Vector2)transform.position + new Vector2(0, -50);
    }

    // NOTE: This function is no longer used, since determining the word spawning position is now delegated to
    // the words themselves, which will attempt to position themselves in a random position where they don't overlap
    // with any other words. The word spawner is now just responsible for word creation.
    // Picks a spawning location for a word to be placed at.
    private Vector2 DetermineWordSpawnLocation()
    {
        /*do
         {
             //spawnPos = new Vector2(Random.Range(-15, 15) * 40, Random.Range(-10, 10) * 40);
             spawnPos = new Vector2(Random.Range(-4, 5) * 150, Random.Range(-8, 9) * 40);
         }
         while (prevSpawnPos.Contains(spawnPos));

         // keeps track of locations of previous words to ensure a word doesn't get spawned on top of another one
         prevSpawnPos.Enqueue(spawnPos);
         if (prevSpawnPos.Count > 10)
         {
             prevSpawnPos.Dequeue();
         }

        //return spawnPos;
        return new Vector2(spawnPos.x + Random.Range(-15, 15), spawnPos.y + Random.Range(-10, 10));
         //return new Vector2(Random.Range(-20, 20) * 40 + Random.Range(-4, 4), 500);*/
        return Vector2.zero;
    }

    // Spawns words on a constant time interval.
    private IEnumerator SpawnWordsOnInterval(float seconds)
    {
        while (true)
        {
            //GameObject word = Instantiate(wordPrefab, DetermineWordSpawnLocation(), Quaternion.identity);
            GameObject word = Instantiate(wordPrefab, Vector2.zero, Quaternion.identity);
            word.GetComponent<WordController>().Initialize(wordListFileName, misspellingChance);
            yield return new WaitForSeconds(seconds);
        }
    }
}
