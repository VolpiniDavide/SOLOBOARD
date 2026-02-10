using UnityEngine;

public class AllyWorldSpawner : MonoBehaviour
{
    public static AllyWorldSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnWorldAlly(AllyData data, Vector3 pos)
    {
        Instantiate(data.worldPrefab, pos, Quaternion.identity);
    }
}
