using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnterableData", menuName = "EnterableData")]
public class ChangeLevel : ScriptableObject
{
    public List<MyClass> RoomEnterableData;    
}

[System.Serializable]
public class MyClass
{
    public BoxCollider changeLevelTrigger;
    public Vector3 spawnPoint;
    public SceneReference newLevel;

    public MyClass(BoxCollider changeLevelTrigger, Vector3 spawnPoint, SceneReference newLevel)
    {
        this.changeLevelTrigger = changeLevelTrigger;
        this.changeLevelTrigger = changeLevelTrigger;
        this.newLevel = newLevel;
    }
}
