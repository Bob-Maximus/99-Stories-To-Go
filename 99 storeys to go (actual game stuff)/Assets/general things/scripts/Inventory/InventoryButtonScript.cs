using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonScript : MonoBehaviour
{
    private void Update()
    {
        CheckIfActive();
    }

    private void CheckIfActive()
    {
        if (gameObject.activeSelf == true)
        {
            Time.timeScale = 1;
        } else
        {
            Time.timeScale = 0;
        }
    }
}
