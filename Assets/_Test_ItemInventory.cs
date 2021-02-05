using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test_ItemInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _SpawnRandomItemToPlayerStash()
    {
        //BasicGear x2 = new BasicGear("Simple Axe", Rarity.COMMON, EquipmentSlot.RIGHT_HAND, 2, 0);
        //BasicGear x2 = LoadItem.Instance.Spawn("Hammer");

        int roll = Random.Range(0, LoadItem.Instance.gearsData.gears.Length);
        BasicGear x = GearJsonData.Spawn(LoadItem.Instance.gearsData.gears[roll]);
        PlayerData.Instance.AddGearToStash(x);
        Debug.Log("Spawn Free item " + x.name);
    }

    public void _TakeRandomItemFromPlayerStash()
    {
        int roll = Random.Range(0, PlayerData.Instance.stashedItems.Count);
        BasicGear x = PlayerData.Instance.stashedItems[roll];
        PlayerData.Instance.RemoveGearFromStash(x);        
    }
}
