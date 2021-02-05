using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load the 3d meshes and put into the inventory
/// </summary>
public class StashUIItemsManager : MonoBehaviour
{
    static StashUIItemsManager _instance;
    public static StashUIItemsManager Instance { get { return _instance; } }

    [SerializeField] GameObject defaultPrefab;

    [SerializeField] float updatePeriod = 2f;
    
    StashSizeControl stashControl;

    Dictionary<BasicGear, GameObject> dataToUIDictionary; // Record that match player data with actual UI

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        dataToUIDictionary = new Dictionary<BasicGear, GameObject>();
    }

    void Start()
    {
        stashControl = FindObjectOfType<StashSizeControl>();        
    }

    void OnEnable()
    {
        // Rearrange
        if(stashControl)
            stashControl.RearrangeItems();

        // Populate
        StartCoroutine(UpdateStashCoroutine());
    }

    // Periodically call UpdateStashItems
    IEnumerator UpdateStashCoroutine()
    {
        yield return null;
        while (true)
        {
            UpdateStashItems();
            yield return new WaitForSeconds(updatePeriod);
        }
    }

    // Update Stash, look through existing uis and  Spawn UI Item into each slot, following players basicGear inventory if necessary
    void UpdateStashItems()
    {
        // Loop through each gear in player stash, and initiate them 
        if (PlayerData.Instance)
        {
            // 1. Make sure all gear is within UI
            foreach (BasicGear gear in PlayerData.Instance.stashedItems)
            {
                // Check if UI contain this already, if so skip. Else, spawn
                if (!dataToUIDictionary.ContainsKey(gear))
                {
                    GameObject prefabToLoad = LoadItem.Instance.GetPrefabByName(gear.name);
                    if (prefabToLoad == null)
                        prefabToLoad = defaultPrefab;
                    // Todo decide UI Effects
                    
                    // Instantiate, create proper scale & add to slot
                    GameObject uiItem = Instantiate(prefabToLoad);
                    stashControl.AddItem(uiItem);

                    // Add to dict
                    dataToUIDictionary.Add(gear, uiItem);
                }
            }

            // 2. Remove UI item that is not within player inventory
            List<BasicGear> toRemove = new List<BasicGear>();
            foreach (BasicGear gear in dataToUIDictionary.Keys)
                if (!PlayerData.Instance.stashedItems.Contains(gear))
                {
                    GameObject uiItemToRemove = dataToUIDictionary[gear];
                    Destroy(uiItemToRemove);        // Destroy item
                }

            // Remove dict separateluy
            foreach (BasicGear gear in toRemove)
                dataToUIDictionary.Remove(gear);  
        }
    }

    // Reverse get of dataToUIDictionary
    public BasicGear GetDataFromUIgameObject(GameObject uiGameObject)
    {
        foreach (BasicGear gear in dataToUIDictionary.Keys)
            if (dataToUIDictionary[gear] == uiGameObject)
                return gear;

        return null;
    }
}
