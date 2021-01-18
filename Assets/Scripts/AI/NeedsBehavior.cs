using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the dynamics of all needs numbers
/// </summary>
public class NeedsBehavior : MonoBehaviour
{
    [SerializeField] Needs _needs;

    void Start()
    {

        InitNeeds();
    }

    void InitNeeds()
    {
        // Initialize
        this._needs = new Needs();
        _needs.BloodLust = 80 + Random.Range(-10, 10);
        _needs.Shopping = 90 + Random.Range(-10, 10);
        _needs.Energy = 90 + Random.Range(-15, 15);
        _needs.HP = 100;
    }

    void Update()
    {
        FrameUpdateEnergy();
        FrameUpdateBloodLust();
        FrameUpdateShopping();
    }

    
    // Energy will decay over time (Full life = 5mins). Will also decay after combat
    void FrameUpdateEnergy()
    {
        float energyTotalDuration = 300f;  // in sec  // 5 mins to go from 100 to 0
        float rate = 100f / energyTotalDuration;
        _needs.Energy -= Time.deltaTime * rate;
    }    

    // Decay over time, floor at 20
    void FrameUpdateBloodLust()
    {
        float totDuration = 300f;
        float rate = 100f * totDuration;
        _needs.BloodLust -= Time.deltaTime * rate;
        _needs.BloodLust = Mathf.Max(20f, _needs.BloodLust); // Floor at 20
    }

    // Shopping will decay over time (Full life = 10mins). Will also decay after combat
    void FrameUpdateShopping()
    {
        float totDuration = 600f;
        float rate = 100f / totDuration;
        _needs.Shopping -= Time.deltaTime * rate;
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
}

