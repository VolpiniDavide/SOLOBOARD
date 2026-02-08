using UnityEngine;

public class Unit : MonoBehaviour
{
    UnitData data;

    public void Init(UnitData d)
    {
        data = d;

        Debug.Log($"Spawned {d.unitName} HP:{d.hp}");
    }
}
