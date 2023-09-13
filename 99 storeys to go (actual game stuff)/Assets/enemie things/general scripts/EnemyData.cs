using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "melee enemy data", menuName = "Character Data/enemie data/melee enemy data")]
public class EnemyData : ScriptableObject
{
    public string enemieName;

    public float damage;
    public float knockBack;
    public float speed;
    public float attackSpeed;
    public float attachrange;
    public int sightrange;
    public float maxHealth;
    public bool canPickObjectsUp;

    public List<LayerMask> attackLayer;
    public List<ItemData> drops;

    public AnimatorOverrideController animOverrides;

    [HideInInspector]
    public bool isMelee = true;
}
