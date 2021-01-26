using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    public int Level { get { return level; } }
}
