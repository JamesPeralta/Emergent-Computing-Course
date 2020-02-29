using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string team;
    public float timeBeetweenSpawns;
    public PlayerController[] greenChars;
    public PlayerController[] redChars;
    private PlayerController[] myChars;
    public int maxChars;
    float timeSinceLastSpawn;

    // Start is called before the first frame update
    private void Start()
    {
        if (team == "Slytherin")
        {
            myChars = greenChars;
        }
        else
        {
            myChars = redChars;
        }
    }

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBeetweenSpawns)
        {
            GameObject[] getCount = GameObject.FindGameObjectsWithTag(team);
            if (getCount.Length < maxChars)
            {
                timeSinceLastSpawn -= timeBeetweenSpawns;
                SpawnStuff();
            }
        }
    }

    // Update is called once per frame
    void SpawnStuff()
    {
        PlayerController prefab = myChars[0];
        PlayerController spawn = Instantiate<PlayerController>(prefab);
        spawn.transform.position = transform.position;
    }
}
