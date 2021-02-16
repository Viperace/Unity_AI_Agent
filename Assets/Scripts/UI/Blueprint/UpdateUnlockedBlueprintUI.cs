using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUnlockedBlueprintUI : MonoBehaviour
{
    Button[] buttons;
    public string purposeSubtype { get; set; }

    PopulateDetailedBlueprintUI populateDetailedBlueprintUI;

    Dictionary<Button, HashSet<Blueprint>> buttonsItemsDictionary; // Each button correspond to how many items

    void Start()
    {
        buttons = GetComponentsInChildren<Button>(true);

        buttonsItemsDictionary = new Dictionary<Button, HashSet<Blueprint>>();

        populateDetailedBlueprintUI = FindObjectOfType<PopulateDetailedBlueprintUI>();

        Invoke("Initialize", 0.6f); // Cant use courtine, cuz it will be disabled
        //StartCoroutine(Initialize(0.6f));
    }

    void Initialize()
    {
        // Attach button to full blueprint list. Regardless of player unlocked
        if(purposeSubtype != null) 
        {
            for (int i = 0; i < System.Enum.GetNames(typeof(Rarity)).Length; i++) // Loop rarity
            {
                // Get all possible blueprints for particular Rariy and tag to the dictionary
                Rarity rarity = (Rarity)i;
                List<Blueprint> blueprints = Blueprint.GetBlueprintOfSubtypesAndRarity(purposeSubtype, rarity);
                buttonsItemsDictionary.Add(buttons[i], new HashSet<Blueprint>(blueprints));
            }

            // Attach function to button
            foreach (Button btn in buttons)
            {
                Blueprint[] array = new Blueprint[buttonsItemsDictionary[btn].Count];
                buttonsItemsDictionary[btn].CopyTo(array);

                btn.onClick.AddListener(() => populateDetailedBlueprintUI.PopulateButton(array));
            }
        }

    }

    float cooldownPeriod = 0.5f;
    float _cooldown = 0;
    List<Blueprint> _allBlueprints;
    List<Blueprint> _playerBlueprints;
    void Update()
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown < 0 && PlayerData.Instance && Blueprint.BlueprintsDictionary != null)
        {
            _cooldown = cooldownPeriod;

            // Get all blueprints for this subtype
            _allBlueprints = Blueprint.GetBlueprintOfSubtypes(purposeSubtype);

            // Find how many of it the player has unlocked
            _playerBlueprints = new List<Blueprint>();
            foreach(Blueprint b in PlayerData.Instance.unlockedBlueprints)
            {
                foreach(Blueprint a in _allBlueprints)
                    if (b.name.Equals(a.name))
                        _playerBlueprints.Add(b);
            }

            // Determine which rarity is unlocked, for this subtype
            HashSet<Rarity> unlockedSet = new HashSet<Rarity>();
            foreach(Blueprint b in _playerBlueprints)
                unlockedSet.Add(b.rarity);                

            // Set the button gray
            for(int i = 0; i < System.Enum.GetNames(typeof(Rarity)).Length; i++)
            {
                Rarity rarityToCheck = ((Rarity)i);
                bool isUnlocked = unlockedSet.Contains(rarityToCheck);
                ToggleButton(isUnlocked, buttons[i]);
            }
        }

        // Change the UI to show disable/enable state
        void ToggleButton(bool isEnable, Button button)
        {
            if (isEnable)
            {
                button.interactable = true;
                button.GetComponentInChildren<Image>().color = Color.white;
            }
            else
            {
                button.interactable = false;
                button.GetComponentInChildren<Image>().color = Color.gray;
            }
            
        }
    }
}
