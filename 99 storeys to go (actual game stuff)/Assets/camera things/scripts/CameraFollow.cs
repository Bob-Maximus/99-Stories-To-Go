using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraFollow : MonoBehaviour
{
    public CameraData cameraData;

    public List<GameObject> targets;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (cameraData.addObjectsWithTag != null)
        {
            targets = GameObject.FindGameObjectsWithTag(cameraData.addObjectsWithTag).Select(go => go.gameObject).ToList();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int i = 0;
        while (i < cameraData.cameraFollow.Count)
        {
            targets.Add(cameraData.cameraFollow[i]);
            i++;
        }

        Move();
        Zoom();
        if(cameraData.topLimit != 0 || cameraData.bottomLimit != 0 || cameraData.leftLimit != 0 || cameraData.rightLimit != 0)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraData.leftLimit, cameraData.rightLimit); Mathf.Clamp(transform.position.y, cameraData.topLimit, cameraData.bottomLimit);)
        }
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(cameraData.minimumZoom, cameraData.maximumZoom, GetGreatestDistance() / cameraData.zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * cameraData.cameraSpeed);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].transform.position);
            }
        }

        return bounds.size.x;
    }

    private void Move()
    {
        cameraData.cameraOffset = new Vector3(0, cameraData.cameraHeight, cameraData.cameraZoom);
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + cameraData.cameraOffset;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraData.cameraSpeed);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].transform.position;
        }

        var bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].transform.position);
            }
        }
       
        Debug.Log(bounds.center);
        return bounds.center;
    }
}
