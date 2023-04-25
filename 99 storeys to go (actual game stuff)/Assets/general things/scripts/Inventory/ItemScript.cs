using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    public Text countText;

    [Header("UI")]

    [HideInInspector] public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public int count = 1;

    public void Start()
    {
        InitializeItem(itemData);
    }

    public void InitializeItem(ItemData item)
    {
        itemData = item;
        image.sprite = item.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if ()

        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
