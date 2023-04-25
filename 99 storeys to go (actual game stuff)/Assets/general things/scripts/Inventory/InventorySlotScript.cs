using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    private ItemData itemData;

    public SlotType type;

    [HideInInspector] public bool isSelected;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        
    }

    public void Deselect()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            ItemScript item = eventData.pointerDrag.GetComponent<ItemScript>();
            item.parentAfterDrag = transform;
        }
    }

    public enum SlotType
    {
        anything = 1, weapon = 2, consumable = 3, currency = 4, material = 5, armour = 6
    }
}
