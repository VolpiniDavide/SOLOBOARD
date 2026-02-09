using Pathfinding;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int strength;
    public int speed;
    public int resistance;

    public void Init(Transform target)
    {
        GetComponent<AIDestinationSetter>().target = target;

        var ai = GetComponent<AIPath>();
        ai.maxSpeed = speed;
    }
}
