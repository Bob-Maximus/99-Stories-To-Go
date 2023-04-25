using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "player stats", menuName = "Character Data/Player stats")]
public class PlayerData : ScriptableObject
{
    [Header("stats")]

    public float speed;
    public float jumpHeight;
    public float maxHealth;
    public bool canPickObjectsUp;

    public LayerMask goundLayer;

    [HideInInspector]
    public bool isGrounded;
}
