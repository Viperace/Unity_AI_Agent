using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTrading
{
    public MarketTrading() { }

    public static bool PlayerBuy(Commodity commodity, int quantity)
    {
        int askPrice = Market.Instance.GetBidAsk(commodity)[1];
        int totalCost = quantity * askPrice;

        // Got money to pay?
        if (PlayerData.Instance.gold < totalCost)
            return false;
        else  // Pay
            PlayerData.Instance.gold -= totalCost;

        // Receive
        PlayerData.Instance.AddCommodity(commodity, quantity);

        return true;
    }

    public static bool PlayerSell(Commodity commodity, int quantity)
    {
        int bidPrice = Market.Instance.GetBidAsk(commodity)[0];
        int totalCost = quantity * bidPrice;

        // Got quantity  to sell?
        if (PlayerData.Instance.GetCommodityQuantity(commodity) < quantity)
            return false;
        else  // Pay
            PlayerData.Instance.AddCommodity(commodity, -quantity);

        // Receive Gold
        PlayerData.Instance.gold += totalCost;

        return true;
    }
}
