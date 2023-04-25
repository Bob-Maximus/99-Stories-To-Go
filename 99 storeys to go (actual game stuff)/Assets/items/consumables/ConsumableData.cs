using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "consumable", menuName = "object data/consumable data")]
public class ConsumableData : ScriptableObject
{
    public string name;

    public Effect effect;
    public float useTime;
    public float severity;

    public enum Effect
    {
        healing = 1, poison = 2
    }
}
