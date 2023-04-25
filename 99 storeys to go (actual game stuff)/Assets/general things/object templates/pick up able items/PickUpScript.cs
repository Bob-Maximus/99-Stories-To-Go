using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public ItemData itemData;

    private GameObject inventoryManager;

    private void Start()
    {
        var model = Instantiate(itemData.model, transform.position, transform.rotation);
        model.parent = transform;

        inventoryManager = GameObject.FindGameObjectWithTag("inventory manager");

        if (itemData.tilt == true)
        {
            model.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 25);
        }
    }

    private void Update()
    {
        Spin();
    }

    private void Spin()
    {
        Transform model = GetComponentInChildren<Transform>();
        model.transform.Rotate(0, Time.deltaTime * 80, 0);
    }

    private void GetPickedUp()
    {
        InventoryManager invenScript = inventoryManager.GetComponent<InventoryManager>();
        bool result = invenScript.AddItem(itemData);
        if (result == true)
        {
            Destroy(gameObject);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetPickedUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Enviroment"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
