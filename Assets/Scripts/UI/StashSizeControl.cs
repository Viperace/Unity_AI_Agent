using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StashSizeControl : MonoBehaviour
{
    public int stashSizePerScreen = 8;
    public RectTransform slotPrefab;
    RectTransform scrollPanel;
    
    void Start()
    {
        scrollPanel = transform.Find("ScrollPanel").GetComponent<RectTransform>();

    }

    // Remove all items
    public void Clear()
    {
        StashSlot[] slots = scrollPanel.GetComponentsInChildren<StashSlot>();
        foreach (StashSlot s in slots)
            if (s.IsOccupied())
                s.DestroyItem();

        RemoveEmptySlots();
    }

    // Find top-most empty slot
    public StashSlot GetEmptySlot()
    {
        StashSlot[] slots = scrollPanel.GetComponentsInChildren<StashSlot>();
        foreach (StashSlot s in slots)
            if (!s.IsOccupied())
                return s;

        // If all are occupied, then generate new empty slot
        ExpandSizeForOneScreen();
        StashSlot[] expandedSlots = scrollPanel.GetComponentsInChildren<StashSlot>();
        return expandedSlots[slots.Length];
    }

    public void AddItem(GameObject go)
    {        
        // Parent
        StashSlot slot = GetEmptySlot();
        go.transform.SetParent(slot.transform);

        // Fix pos & scale
        go.transform.localPosition = new Vector3(0, 0, -100);
        go.transform.localScale = Vector3.one * 150; 

        // Add component
        go.AddComponent<RotateIfOnUI>();
        //go.AddComponent<DragTransform>();
        if (go.GetComponentInChildren<MeshRenderer>())
            go.GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<DragTransform>();
    }

    public void ExpandSizeForOneScreen()
    {
        // Add slots
        for(int i = 0; i < stashSizePerScreen; i++)
        {
            RectTransform newSlot = Instantiate(slotPrefab, scrollPanel);
        }

        // Increase width
        scrollPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollPanel.sizeDelta.x + Screen.width + 50);
    }

    /// <summary>
    /// Move all items to occupied leftmost slots
    /// </summary>
    public void RearrangeItems()
    {
        StashSlot[] slots = scrollPanel.GetComponentsInChildren<StashSlot>();
        List<RotateIfOnUI> items = new List<RotateIfOnUI>();
        foreach(StashSlot slot in slots)
        {
            // Check if got item, if so, unparent it
            RotateIfOnUI item = slot.GetComponentInChildren<RotateIfOnUI>();
            if (item)
            {
                items.Add(item);
                //item.transform.SetParent(null);
            }
        }

        // Re-attach parent
        for(int i = 0; i < items.Count; i++)
        {
            // Add by index
            items[i].transform.SetParent(slots[i].GetComponent<RectTransform>());
            items[i].transform.localPosition = new Vector3(0, 0, -100);
        }
    }

    /// <summary>
    /// If empty slots > stashSizePerScreen, then delete whole 'stashSizePerScreen' 
    /// </summary>
    public void RemoveEmptySlots()
    {
        // Num of item
        StashSlot[] slots = scrollPanel.GetComponentsInChildren<StashSlot>();
        int totalItems = 0;
        foreach (StashSlot s in slots)
            if(s.IsOccupied())
                totalItems++;

        // Compare with Num of slots
        if(slots.Length - totalItems > stashSizePerScreen)
        {
            // Delete N slots
            int slotsRemoved = 0;
            foreach (StashSlot s in slots)
            {
                if (!s.IsOccupied())
                {
                    Destroy(s.gameObject);
                    slotsRemoved++;
                }

                if (slotsRemoved >= stashSizePerScreen)
                    break;
            }

            // Decrease width
            scrollPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollPanel.sizeDelta.x - Screen.width - 50);
        }

    }
}
