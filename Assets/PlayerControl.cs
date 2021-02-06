using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameObject TradingPanel;
    [SerializeField] GameObject StashPanel;
    [SerializeField] GameObject SalesPanel;

    GameView view;

    void Start()
    {
        view = GameView.WORLD_VIEW;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (view == GameView.COMMODITY_TRADING_VIEW)
                view = GameView.WORLD_VIEW; // Reset back
            else
                view = GameView.COMMODITY_TRADING_VIEW;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (view == GameView.INVENTORY_VIEW)
                view = GameView.WORLD_VIEW; // Reset back
            else
                view = GameView.INVENTORY_VIEW;
        }

        // FIX ME. DONT KEEP SETTING ACTIVE !!!
        SetView(view);
    }

    void SetView(GameView view)
    {
        // Close all menu first
        

        if (view == GameView.WORLD_VIEW)
        {
            StashPanel.SetActive(false);
            SalesPanel.SetActive(false);
            TradingPanel.SetActive(false);
        }
        else if(view == GameView.INVENTORY_VIEW)
        {
            // Open the two            
            StashPanel.SetActive(true);
            SalesPanel.SetActive(true);
            TradingPanel.SetActive(false);
        }
        else if (view == GameView.COMMODITY_TRADING_VIEW)
        {
            StashPanel.SetActive(false);
            SalesPanel.SetActive(false);
            TradingPanel.SetActive(true);
        }

    }
}

enum GameView
{
    WORLD_VIEW = 0,
    COMMODITY_TRADING_VIEW,
    INVENTORY_VIEW,
    OPTION_MENU,
    HEXAGON_GAME_MENU
}