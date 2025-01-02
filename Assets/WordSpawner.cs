using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject wordPrefab;
    private Vector2 spawnPos;
    private Queue<Vector2> prevSpawnPos = new Queue<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(100, 200);
        StartCoroutine(SpawnWordsOnInterval(1f));
    }

    private Vector2 DetermineWordSpawnLocation()
    {
        do
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
         //return new Vector2(Random.Range(-20, 20) * 40 + Random.Range(-4, 4), 500);
    }

    // Spawns words on a constant time interval.
    private IEnumerator SpawnWordsOnInterval(float seconds)
    {
        while (true)
        {
            GameObject word = Instantiate(wordPrefab, DetermineWordSpawnLocation(), Quaternion.identity);
            yield return new WaitForSeconds(seconds);
        }
    }
}
