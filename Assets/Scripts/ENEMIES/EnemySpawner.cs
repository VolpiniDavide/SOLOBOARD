using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public void Spawn()
    {
        var e = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        e.GetComponent<Enemy>().Init(player);
    }
}
