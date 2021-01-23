using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _NeedsEditor : MonoBehaviour
{
    [SerializeField] float Energy;
    [SerializeField] float BloodLust;
    [SerializeField] float Shopping;

    public bool forceEnergy = false;
    public bool forceBloodlust = false;
    public bool forceShopping = false;

    NeedsBehavior needsBehavior;

    void Start()
    {
        needsBehavior = GetComponent<NeedsBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (forceEnergy)
                needsBehavior._ForceSetNeeds(new Needs(needsBehavior.needs.BloodLust, this.Energy, needsBehavior.needs.Shopping, needsBehavior.needs.HP));

            if( forceBloodlust)
                needsBehavior._ForceSetNeeds(new Needs(this.BloodLust, needsBehavior.needs.Energy, needsBehavior.needs.Shopping, needsBehavior.needs.HP));

            if (forceShopping)
                needsBehavior._ForceSetNeeds(new Needs(needsBehavior.needs.BloodLust, needsBehavior.needs.Energy, this.Shopping, needsBehavior.needs.HP));
        }
    }
}
