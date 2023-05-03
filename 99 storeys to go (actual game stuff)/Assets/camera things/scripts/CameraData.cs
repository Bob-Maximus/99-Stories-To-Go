using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cameraData", menuName = "scriptable objects/camera data")]
public class CameraData : ScriptableObject
{
    public string CamName = "camera";

    public float cameraZoom = 7, zoomLimiter = 6;
    public float cameraHeight = 0.5f;
    public float cameraSpeed = 5;
    public float minimumZoom = 30, maximumZoom = 100;

    public List<GameObject> cameraFollow = new List<GameObject>();
    public string addObjectsWithTag = "camera focus";

    [Header("limits")]
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;

    [HideInInspector]
    public Vector3 cameraOffset;
}

