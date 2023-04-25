using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gun data", menuName = "object data/weapon data/gun data/normal gun data")]
public class GunData : ScriptableObject
{
    public string name;

    public float damage;
    public float range;
    public float fireRate;
    public float magSize;
    public float reloadSpeed;
}
