using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] colorprefabs;
    private float Spawnrangex = 9;
    private float SpawnposZ = 150;
    private float StartDelay = 2;
    private float SpawnIntervel = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomColor", StartDelay, SpawnIntervel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnRandomColor()
    {
        int colorIndex = Random.Range(0, colorprefabs.Length);
        Vector3 Spawnpos = new Vector3(Random.Range(-Spawnrangex, Spawnrangex), 0, SpawnposZ);
        Instantiate(colorprefabs[colorIndex], Spawnpos, colorprefabs[colorIndex].transform.rotation);
    }
}
