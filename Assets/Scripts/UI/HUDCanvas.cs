using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCanvas : MonoBehaviour
{
    TMP_Text oreText;
    TMP_Text steelText;
    TMP_Text goldText;
    TMP_Text orichalcumText;
    TMP_Text coalText;

    void Start()
    {
        goldText = GameObject.Find("GoldText").GetComponent<TMP_Text>();
        oreText = GameObject.Find("OreText").GetComponent<TMP_Text>();
        steelText = GameObject.Find("SteelText").GetComponent<TMP_Text>();
        orichalcumText = GameObject.Find("OriText").GetComponent<TMP_Text>();
        coalText = GameObject.Find("CoalText").GetComponent<TMP_Text>();

    }

    void LateUpdate()
    {
        goldText.text = PlayerData.Instance.gold.ToString();
        oreText.text = PlayerData.Instance.ore.ToString();
        steelText.text = PlayerData.Instance.steel.ToString();
        orichalcumText.text = PlayerData.Instance.orichalcum.ToString();
        coalText.text = PlayerData.Instance.coal.ToString();
    }
}
