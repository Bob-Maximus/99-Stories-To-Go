using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlotScript[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject inventory;

    public int selectedSlot = -1;

    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 6)
            {
                ChangeSelectedSlot(number - 1);
            }
        }

        if (inventory.activeSelf == true)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlotScript slot = inventorySlots[i];
            ItemScript itemInSlot = slot.GetComponentInChildren<ItemScript>();
            if (itemInSlot != null && itemInSlot.itemData.name == item.name && itemInSlot.count < item.stackAmount)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlotScript slot = inventorySlots[i];
            ItemScript itemInSlot = slot.GetComponentInChildren<ItemScript>();
            if (itemInSlot == null)
            {
                if ((int)slot.type == (int)item.type || (int)slot.type == 1)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
        }

        return false;
    }

    void SpawnNewItem(ItemData item, InventorySlotScript slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        ItemScript inventoryItem = newItemGo.GetComponent<ItemScript>();
        inventoryItem.InitializeItem(item);
    }

    public ItemData GetSelectedItem()
    {
        InventorySlotScript slot = inventorySlots[selectedSlot];
        ItemScript itemInSlot = slot.GetComponentInChildren<ItemScript>();
        if (itemInSlot != null)
        {
            return itemInSlot.itemData;
        }

        return null;
    }
}
