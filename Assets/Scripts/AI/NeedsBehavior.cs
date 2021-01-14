using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the dynamics of all needs numbers
/// </summary>
public class NeedsBehavior : MonoBehaviour
{
    Needs _needs;

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
        _needs.Survival = 100;
    }

    void Update()
    {
        FrameUpdateEnergy();

        FrameUpdateBloodLust();
    }

    
    void FrameUpdateEnergy()
    {
        float energyTotalDuration = 300f;  // in sec  // 5 mins to go from 100 to 0
        float rate = 100f / energyTotalDuration;
        _needs.Energy -= Time.deltaTime * rate;
    }    

    void FrameUpdateBloodLust()
    {
        float totDuration = 30f;
        float rate = 100f * totDuration;
        _needs.BloodLust -= Time.deltaTime * rate;
        _needs.BloodLust = Mathf.Max(20f, _needs.BloodLust); // Floor at 20
    }

    public Needs needs { get { return _needs; } }

}

