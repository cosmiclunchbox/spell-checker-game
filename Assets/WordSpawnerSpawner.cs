using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawnerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject wordSpawnerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleSpawningPeople());
    }

    // Handles all spawning of people (word spawners).
    private IEnumerator HandleSpawningPeople()
    {
        int positionIndex = 0;
        foreach (WordSpawner.Type type in System.Enum.GetValues(typeof(WordSpawner.Type)))
        {
            //GameObject word = Instantiate(wordPrefab, DetermineWordSpawnLocation(), Quaternion.identity);
            GameObject person = Instantiate(wordSpawnerPrefab, Vector2.zero, Quaternion.identity);
            person.GetComponent<WordSpawner>().Initialize(positionIndex, type);
            positionIndex++;
            yield return new WaitForSeconds(10);
        }
        yield return null;
    }
}
