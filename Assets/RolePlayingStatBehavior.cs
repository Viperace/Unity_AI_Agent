using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePlayingStatBehavior : MonoBehaviour
{
    RolePlayingStat rpgStat;
    LevelUp levelUp;

    void Awake()
    {
        Gender sex = Random.value > 0.5f ? Gender.MALE : Gender.FEMALE;
        rpgStat = RolePlayingStat.GenerateNewProfile(sex);
        levelUp = new LevelUp();
    }

    void Update()
    {
        UpdateLevelUp();
    }

    // Exprience point > Threshold = level up
    void UpdateLevelUp()
    {
        bool doLevelUp = levelUp.ApplyLevelUpOnce(this.rpgStat);
        if (doLevelUp)
        {
            Debug.Log(rpgStat.name + " level up to " + rpgStat.level);
        }
    }

    #region Setters
    public void AddExperiencePoint(float xp) => rpgStat.experiencePoint += xp;
    
    #endregion
    #region Getters
    public RolePlayingStat RPGstat { get { return rpgStat; } }
    public string Name { get { return rpgStat.name; } }
    public int Level { get { return rpgStat.level; } }

    #endregion
}

class LevelUp
{
    int[] xpThreshold;
    int levelCap;
    public LevelUp() 
    {
        levelCap = 10;
        // Threshold
        xpThreshold = new int[] { 3, 8, 14, 23, 35, 70, 105, 140, 180 };
    }
    public bool CanLevelUp(int level, float xp)
    {
        if (level < levelCap)
            return xp >= xpThreshold[level - 1];
        else
            return false;
    }
    public bool ApplyLevelUpOnce(RolePlayingStat stat)
    {
        if (CanLevelUp(stat.level, stat.experiencePoint))
        {
            stat.experiencePoint -= xpThreshold[stat.level = 1];
            stat.level++;
            return true;
        }
        else
            return false;
    }
}