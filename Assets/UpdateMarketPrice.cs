using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateMarketPrice : MonoBehaviour
{
    public TMP_Text tmpText_Ore;
    public TMP_Text tmpText_Steel;
    public TMP_Text tmpText_Ori;

   
    void LateUpdate()
    {
        if (Market.Instance)
        {
            tmpText_Ore.text = string.Concat("Ore:", Market.Instance.GetValue(Commodity.ORE).ToString());
            tmpText_Steel.text = string.Concat("Steel:", Market.Instance.GetValue(Commodity.STEEL).ToString());
            tmpText_Ori.text = string.Concat("Ori:", Market.Instance.GetValue(Commodity.ORICHALCUM).ToString());
        }
    }
}
