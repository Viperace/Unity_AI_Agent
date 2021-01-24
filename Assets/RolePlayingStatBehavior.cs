using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RolePlayingStatBehavior : MonoBehaviour
{
    [SerializeField] public RolePlayingStat rpgStat;
    LevelUp levelUp;

    void Awake()
    {
        Gender sex = Random.value > 0.5f ? Gender.MALE : Gender.FEMALE;
        rpgStat = RolePlayingStat.GenerateNewProfile(sex);
        levelUp = new LevelUp();
    }

    void OnEnable()
    {
        StartCoroutine(UpdateLevelUp());
    }

    // Exprience point > Threshold = level up
    IEnumerator UpdateLevelUp()
    {
        while (true)
        {
            levelUp.ApplyLevelUpOnce(this.rpgStat);
            yield return new WaitForSeconds(1f);
        }        
    }

    #region Setters
    public void AddExperiencePoint(float xp) => rpgStat.experiencePoint += xp;
    
    #endregion
    #region Getters
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
            stat.experiencePoint -= xpThreshold[stat.level - 1];
            stat.level++;
            return true;
        }
        else
            return false;
    }
}