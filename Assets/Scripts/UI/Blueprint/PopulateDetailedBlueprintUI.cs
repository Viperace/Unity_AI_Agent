using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateDetailedBlueprintUI : MonoBehaviour
{
    Button[] craftButtons;

    void Start()
    {
        craftButtons = GetComponentsInChildren<Button>();

        // Hide first
        foreach (Button btn in craftButtons)
            btn.gameObject.SetActive(false);
    }

    GearTextDisplay _gearTextDisplay;
    public void PopulateButtonWrong(params Blueprint[] blueprints)
    {
        for(int i = 0; i < craftButtons.Length; i++)
        {
            // Get texts UI
            TMP_Text itemText = craftButtons[i].transform.Find("ItemText").GetComponent<TMP_Text>();
            TMP_Text statText = craftButtons[i].transform.Find("StatText").GetComponent<TMP_Text>();
            TMP_Text costText = craftButtons[i].transform.Find("CostText").GetComponent<TMP_Text>();

            if (i >= blueprints.Length) 
            {
                craftButtons[i].gameObject.SetActive(false);
            }
            else
            {
                craftButtons[i].gameObject.SetActive(true);

                // Populate text
                _gearTextDisplay = new GearTextDisplay(blueprints[i]);
                itemText.text = _gearTextDisplay.ColoredName();
                statText.text = _gearTextDisplay.ColoredStat();
                costText.text = blueprints[i].ore.ToString();
            }
        }

        // Check, prevent overflow
        if(blueprints.Length > craftButtons.Length)
            Debug.LogWarning("PopulateDetailedBlueprintUI: Button number < blueprints to populate ");

    }

    public void PopulateButton(params Blueprint[] buttonTaggedBlueprints)
    {
        // Intersect player VS button tagged
        List<Blueprint> intersectBPs = Intersect(buttonTaggedBlueprints, PlayerData.Instance.unlockedBlueprints);
        

        for (int i = 0; i < craftButtons.Length; i++)
        {
            // Get texts UI
            TMP_Text itemText = craftButtons[i].transform.Find("ItemText").GetComponent<TMP_Text>();
            TMP_Text statText = craftButtons[i].transform.Find("StatText").GetComponent<TMP_Text>();
            TMP_Text costText = craftButtons[i].transform.Find("CostText").GetComponent<TMP_Text>();

            if (i >= intersectBPs.Count)
            {
                craftButtons[i].gameObject.SetActive(false);
            }
            else
            {
                craftButtons[i].gameObject.SetActive(true);

                // Populate text
                _gearTextDisplay = new GearTextDisplay(intersectBPs[i]);
                itemText.text = _gearTextDisplay.ColoredName();
                statText.text = _gearTextDisplay.ColoredStat();
                costText.text = intersectBPs[i].ore.ToString();
            }
        }

        // Check, prevent overflow
        if (buttonTaggedBlueprints.Length > craftButtons.Length)
            Debug.LogWarning("PopulateDetailedBlueprintUI: Button number < blueprints to populate ");

    }


    public List<Blueprint> Intersect(Blueprint[] buttonSet, HashSet<Blueprint> playerSet)
    {
        // Intersect player VS button tagged
        List<Blueprint> intersectBPs = new List<Blueprint>();
        foreach (Blueprint buttonBp in buttonSet)
            foreach (Blueprint playerBp in playerSet)
                if (playerBp.name == buttonBp.name)
                    intersectBPs.Add(buttonBp);
        return intersectBPs;
    }
}
