using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestCrafting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartCraftingGame()
    {
        PolygonUIGame game = FindObjectOfType<PolygonUIGame>();
        game.NewGame(7);
    }
}
