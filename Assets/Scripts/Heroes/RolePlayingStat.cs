using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RolePlayingStat
{
    public string name;
    public int level;
    public float experiencePoint;
    // To add: classType, race;
    public RolePlayingStat()
    {
        name = "Unnamed";
        level = 1;
        experiencePoint = 0;
    }

    public static RolePlayingStat GenerateNewProfile(Gender sex, int level = 1)
    {
        RolePlayingStat stat = new RolePlayingStat();
        stat.name = NameGenerator.Instance.GenerateRandomName(sex);
        stat.level = level;
        return stat;
    }
    // enum Class
    // BasePower
    // BaseSpeed
}

public class RolePlayingRecord
{
    public int kills;
    public int numberOfTimesFlee;
    public int numberOfTimesVisitTown;

    public RolePlayingRecord()
    {
        kills = 0;
        numberOfTimesFlee = 0;
        numberOfTimesVisitTown = 0;
    }
}
