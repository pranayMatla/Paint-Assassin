using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject[] colorprefabs;
    private float spawnRangeX = 20;
    private float spawnPosZ = 500;
    private float startDelay = 2;
    private float spawnInterval = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomColor", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnRandomColor()
    {
        int colorIndex = Random.Range(0, colorprefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(colorprefabs[colorIndex], spawnPos, colorprefabs[colorIndex].transform.rotation);
    }
}
