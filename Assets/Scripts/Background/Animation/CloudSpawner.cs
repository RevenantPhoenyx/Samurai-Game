using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudPrefabs;
    private float yPosition1 = 1.9f;
    private float yPosition2 = -0.1f;
    private float cloudSpacing = 6.7f;
    private float spawnX = -4f;
    private float cloudSpeed = 1f;      // must match the speed in Clouds.cs
    private int yIndex = 0;

    private void Start()
    {
        SpawnInitialClouds();
        Invoke(nameof(SpawnCloud), cloudSpacing / cloudSpeed); // consistent timing
    }

    private void SpawnInitialClouds()
    {
        int cloudCount = 15;
        for (int i = 0; i < cloudCount; i++)
        {
            int index = i % cloudPrefabs.Length;
            float chosenY = (i % 2 == 0) ? yPosition1 : yPosition2;
            float xPos = spawnX + (i * cloudSpacing);
            Vector3 spawnPos = new Vector3(xPos, chosenY, 0);
            Instantiate(cloudPrefabs[index], spawnPos, Quaternion.identity);
        }
    }

    private void SpawnCloud()
    {
        int index = yIndex % cloudPrefabs.Length;
        float chosenY = (yIndex % 2 == 0) ? yPosition1 : yPosition2;
        yIndex++;
        Instantiate(cloudPrefabs[index], new Vector3(spawnX, chosenY, 0), Quaternion.identity);
        Invoke(nameof(SpawnCloud), cloudSpacing / cloudSpeed); // spawns every 6.7 seconds
    }
}