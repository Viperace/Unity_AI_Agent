using UnityEngine;
using System.Collections;

/// <summary>
/// This class manage the dynamics of all needs numbers
/// </summary>
public class NeedsBehavior : MonoBehaviour
{
    [SerializeField] Needs _needs;
    [SerializeField] bool _freezeBloodLust = false;
    [SerializeField] bool _freezeEnergy = false;
    Inventory inventory;

    void Start()
    {
        InitNeeds();
        inventory = GetComponent<Inventory>();
    }
    void OnEnable()
    {
        // Init counter
        StartCoroutine(SlowUpdateShopping(2f));
    }
    void InitNeeds()
    {
        // Initialize
        this._needs = new Needs();
        _needs.BloodLust = 40 + Random.Range(-10, 10);
        _needs.Shopping = 90 + Random.Range(-10, 10);
        _needs.Energy = 90 + Random.Range(-15, 15);
        _needs.HP = 100;
    }

    void Update()
    {
        if (!_freezeEnergy) FrameUpdateEnergy();
        if (!_freezeBloodLust) FrameUpdateBloodLust();
        if (true) FrameUpdateShopping();

        _needs = CapFloor(_needs);
    }

    
    // Energy will decay over time (Full life = 5mins). Will also decay after combat
    void FrameUpdateEnergy()
    {
        float energyTotalDuration = 300f;  // in sec  // 5 mins to go from 100 to 0
        float rate = 100f / energyTotalDuration;
        _needs.Energy -= Time.deltaTime * rate;
    }

    // Decay over time, floor at 20
    float bloodLustRate = 0;
    void FrameUpdateBloodLust()
    {
        float totDuration = 300f;

        if(_needs.BloodLust > 50f)
            bloodLustRate = 100f / totDuration;
        else if(_needs.BloodLust > 50f)
            bloodLustRate = 50f / totDuration;
        else if (_needs.BloodLust > 20f)
            bloodLustRate = 10f / totDuration;
        else
            bloodLustRate = 1f / totDuration;
        _needs.BloodLust -= Time.deltaTime * bloodLustRate;

        //_needs.BloodLust = Mathf.Max(20f, _needs.BloodLust); // Floor at 20
    }

    /// <summary>
    /// Shopping will decay over time (Full life = 10mins); and is dependent on Gear Duratbility (which decay after combat)
    /// </summary>
    float shoppingDecayRate = 100f / 600f;
    void FrameUpdateShopping() => _needs.Shopping -= Time.deltaTime * shoppingDecayRate;
    
    float _lastDurability = 1f;
    IEnumerator SlowUpdateShopping(float waitDur)
    {
        while (true)
        {
            if (inventory != null)
            {
                float dropInDurability = inventory.MinimumDurability - _lastDurability; // Check lost in durability since last update
                _needs.Shopping -= dropInDurability * 100f; // Update the drops into the Shopping needs
                _lastDurability = inventory.MinimumDurability; // Update durability
            }
            yield return new WaitForSeconds(waitDur);
        }        
    }

    public Needs needs { get { return _needs; } }

    public void AddNeeds(Needs addNeeds)
    {
        _needs += addNeeds;
        _needs = CapFloor(_needs);
    }

    Needs CapFloor(Needs x, float floor = 0, float cap = 100)
    {
        Needs xcapfloor = x;
        xcapfloor.BloodLust = Mathf.Min(Mathf.Max(xcapfloor.BloodLust, floor), cap);
        xcapfloor.Energy = Mathf.Min(Mathf.Max(xcapfloor.Energy, floor), cap);
        xcapfloor.HP = Mathf.Min(Mathf.Max(xcapfloor.HP, floor), cap);
        xcapfloor.Shopping = Mathf.Min(Mathf.Max(xcapfloor.Shopping, floor), cap);

        return xcapfloor;
    }

    public void _ForceSetNeeds(Needs needs) => this._needs = needs;
}

