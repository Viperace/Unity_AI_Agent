using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate equipment over time
/// </summary>
public class ShopAI : MonoBehaviour
{
    Merchant shop;
    void Start()
    {
        // Init property
        shop = GetComponent<Merchant>();

        // Start coroutine
        StartCoroutine(PeriodicGearGenerator());
    }

    void GenerateRandomGearAndPutToShop()
    {        
        BasicGear gear = LoadItem.Instance.SpawnRandomItem();
        shop.AddGearsForSale(gear);
    }


    IEnumerator PeriodicGearGenerator()
    {
        float period = 5f;
        float periodVar = 2f;
        while (true)
        {
            GenerateRandomGearAndPutToShop();
            yield return new WaitForSeconds(period + Random.Range(-periodVar, periodVar));
        }
    }

}

public interface Ishopper
{
    public List<BasicGear> DecideShoppingList(List<BasicGear> allGears);
    public bool Pay(int amount);
    public void TakeItems(List<BasicGear> gears);
}