using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateMarketPrice : MonoBehaviour
{
    public TMP_Text tmpText_Ore;
    public TMP_Text tmpText_Steel;
    public TMP_Text tmpText_Ori;

    public TMP_Text tmpText_BidAsk_Ore;
    public TMP_Text tmpText_BidAsk_Steel;
    public TMP_Text tmpText_BidAsk_Oricalchum;

    public TMP_Text tmpText_playerOre;
    public TMP_Text tmpText_playerSteel;
    public TMP_Text tmpText_playerOri;


    int[] _tmpba;
    void LateUpdate()
    {
        if (Market.Instance)
        {
            tmpText_Ore.text = string.Concat("Ore:", Market.Instance.GetValue(Commodity.ORE).ToString());
            tmpText_Steel.text = string.Concat("Steel:", Market.Instance.GetValue(Commodity.STEEL).ToString());
            tmpText_Ori.text = string.Concat("Ori:", Market.Instance.GetValue(Commodity.ORICHALCUM).ToString());

            _tmpba = Market.Instance.GetBidAsk(Commodity.ORE);
            tmpText_BidAsk_Ore.text = string.Concat(_tmpba[1], " / ", _tmpba[0]);

            _tmpba = Market.Instance.GetBidAsk(Commodity.STEEL);
            tmpText_BidAsk_Steel.text = string.Concat(_tmpba[1], " / ", _tmpba[0]);

            _tmpba = Market.Instance.GetBidAsk(Commodity.ORICHALCUM);
            tmpText_BidAsk_Oricalchum.text = string.Concat(_tmpba[1], " / ", _tmpba[0]);
        }

        if (PlayerData.Instance)
        {

            tmpText_playerOre.text = PlayerData.Instance.ore.ToString();
            tmpText_playerSteel.text = PlayerData.Instance.steel.ToString();
            tmpText_playerOri.text = PlayerData.Instance.orichalcum.ToString();

        }
    }
}
