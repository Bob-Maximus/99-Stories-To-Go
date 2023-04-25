using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item data", menuName = "item data")]
public class ItemData : ScriptableObject
{
    public string name;

    [Header("only gameplay")]
    public ItemType type;
    public enum ItemType
    {
        weapon = 2, consumable = 3, currency = 4, material = 5
    }

    public ActionType actionType;
    public enum ActionType
    {
        attack, consume, spend, craft
    }

    public AnimatorOverrideController animOverrides;
    public Transform model;
    public bool tilt;

    [Header("only UI")]
    public float stackAmount = 1;

    [Header("both")]
    public Sprite image;

    [Header("only for weapons")]
    public float damage;
    public float knockBack;
    public float distance;
    public float fireRate;

    [Header("only for ranged weapons (also used the section above)")]
    public bool isRanged;
    public float magSize;
    public float reloadSpeed;


    [Header("only consumables")]
    public Effect effect;
    public enum Effect
    {
        healing = 1, poison = -1
    }

    public float useTime;
    public float severity;

    [Header("only currency")]
    public CurrencyType currencyType;
    public enum CurrencyType
    {
        anything = 1, gold = 2
    }

    public float Value;
}
