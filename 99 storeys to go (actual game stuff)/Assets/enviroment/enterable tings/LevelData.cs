using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public Sprite backDrop;
    public List<MyClass> changeLevelData;    
}

[System.Serializable]
public class MyClass
{
    public float iconOffset;
    public GameObject changeLevelTrigger;
    public Vector3 spawnPoint;
    public SceneReference newLevel;
    public bool needsInput = false;

    public MyClass(GameObject changeLevelTrigger, Vector3 spawnPoint, SceneReference newLevel, bool needsInput, float iconOffset)
    {
        this.iconOffset = iconOffset;
        this.changeLevelTrigger = changeLevelTrigger;
        this.spawnPoint = spawnPoint;
        this.newLevel = newLevel;
        this.needsInput = needsInput;
    }
}
