using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;

    public void Spawn(UnitData data, Vector3 position)
    {
        GameObject go = Instantiate(unitPrefab, position, Quaternion.identity);

        Unit unit = go.GetComponent<Unit>();

        unit.Init(data);
    }
}
