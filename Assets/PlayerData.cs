using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    static PlayerData _instance;
    public static PlayerData Instance { get { return _instance; } }

    public string playerName;
    public int level;
    public int gold;
    public int ore;
    public int steel;
    public int orichalcum;

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
}
