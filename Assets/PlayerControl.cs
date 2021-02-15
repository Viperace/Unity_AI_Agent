using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameObject TradingPanel;
    [SerializeField] GameObject StashPanel;
    [SerializeField] GameObject SalesPanel;
    [SerializeField] GameObject BlueprintTreePanel;
    [SerializeField] GameObject HUDpanel;
  
    GameView view;

    void Start()
    {
        view = GameView.WORLD_VIEW;
    }

    // Update is called once per frame
    void Update()
    {
        // Escape everything
        // TODO: Game view cnanot be escape !
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Press esacape");
            view = GameView.WORLD_VIEW; 
        }

        // Only responsive on These views
        if (view == GameView.INVENTORY_VIEW || view == GameView.COMMODITY_TRADING_VIEW || view == GameView.WORLD_VIEW
             || view == GameView.BLUEPRINT_TREE_VIEW)
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
            else if (Input.GetKeyDown(KeyCode.B))
            {
                if (view == GameView.BLUEPRINT_TREE_VIEW)
                    view = GameView.WORLD_VIEW;
                else
                    view = GameView.BLUEPRINT_TREE_VIEW;
            }
        }

        SetView(view);
    }

    void SetView(GameView view)
    {
        // Close all menu first
        if (view == GameView.WORLD_VIEW)
        {
            TogglePanelsActive(false, StashPanel, SalesPanel, TradingPanel, BlueprintTreePanel);
            TogglePanelsActive(true, HUDpanel);
        }
        else if (view == GameView.BLUEPRINT_TREE_VIEW)
        {
            TogglePanelsActive(false, StashPanel, SalesPanel, TradingPanel, HUDpanel);

            BlueprintTreePanel.SetActive(true);
        }
        else if(view == GameView.INVENTORY_VIEW)
        {
            TogglePanelsActive(true, StashPanel, SalesPanel, HUDpanel);

            TogglePanelsActive(false, TradingPanel, BlueprintTreePanel);            
        }
        else if (view == GameView.COMMODITY_TRADING_VIEW)
        {
            TogglePanelsActive(false, StashPanel, SalesPanel, BlueprintTreePanel, HUDpanel);

            TradingPanel.SetActive(true);
        }

    }

    void TogglePanelsActive(bool value, params GameObject[] panels)
    {
        foreach (GameObject panel in panels)
            panel.SetActive(value);
    }
}

enum GameView
{
    WORLD_VIEW = 0,
    COMMODITY_TRADING_VIEW,
    INVENTORY_VIEW,
    BLUEPRINT_TREE_VIEW,
    OPTION_MENU,
    HEXAGON_GAME_MENU
}