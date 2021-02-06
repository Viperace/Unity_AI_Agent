using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradingUI : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayerBuyOre() => MarketTrading.PlayerBuy(Commodity.ORE, 1);
    public void PlayerBuySteel() => MarketTrading.PlayerBuy(Commodity.STEEL, 1); 
    public void PlayerBuyOrichalcum() => MarketTrading.PlayerBuy(Commodity.ORICHALCUM, 1);
    public void PlayerBuyCoal() => MarketTrading.PlayerBuy(Commodity.COAL, 1);
    public void PlayerSellOre() => MarketTrading.PlayerSell(Commodity.ORE, 1);
    public void PlayerSellSteel() => MarketTrading.PlayerSell(Commodity.STEEL, 1);
    public void PlayerSellOrichalcum() => MarketTrading.PlayerSell(Commodity.ORICHALCUM, 1);
    public void PlayerSelloal() => MarketTrading.PlayerSell(Commodity.COAL, 1);

}
