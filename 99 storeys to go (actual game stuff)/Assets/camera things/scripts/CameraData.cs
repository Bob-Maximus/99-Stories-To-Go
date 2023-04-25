using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cameraData", menuName = "camera data")]
public class CameraData : ScriptableObject
{
    public string CamName;

    public float cameraZoom;
    public float cameraHeight;
    public float cameraSpeed;
    public float minimumZoom;
    public float maximumZoom;
    public float zoomLimiter;

    public List<GameObject> cameraFollow = new List<GameObject>();
    public string addObjectsWithTag;
    //public GameObject look;

    [HideInInspector]
    public Vector3 cameraOffset;
}

