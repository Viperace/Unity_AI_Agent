using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creeps : Actor
{
    [SerializeField] int level = 1;
    Lair home;

    void Start()
    {
        this.WanderAround(this.transform.position, 8, -1);
    }

    public void SetHome(Lair lair) => home = lair;

    public void GoHome()
    {
        System.Action killSelf = () =>
        {
            home.Deregister(this);
            Destroy(this.gameObject);
        };

        this.GotoTarget(home.gameObject, killSelf, home.enterRadius);
    }
}
