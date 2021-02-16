using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateBlueprintUI : MonoBehaviour
{
    [SerializeField] RectTransform bodyPanel;
    List<UpdateUnlockedBlueprintUI> mainRows;
    List<TMP_Text> tmpTexts;

    void Start()
    {
        // Find all the relevant rows
        mainRows = new List<UpdateUnlockedBlueprintUI>( bodyPanel.GetComponentsInChildren<UpdateUnlockedBlueprintUI>(true));

        // For each rows, extract text
        tmpTexts = new List<TMP_Text>();
        foreach (UpdateUnlockedBlueprintUI row in mainRows)
        {
            TMP_Text text = row.GetComponentInChildren<TMP_Text>(true);
            tmpTexts.Add(text);
        }

        StartCoroutine(InitializeBlueprintSubtypeText());
    }

    IEnumerator InitializeBlueprintSubtypeText()
    {
        string[] subtypes = null;
        while (subtypes == null || subtypes.Length == 0)
        {
            // Initialize
            Blueprint p = new Blueprint();
            subtypes = Blueprint.GetUniqueSubtypes();

            // Populate subtype texts
            for (int i = 0; i < tmpTexts.Count; i++)
            {
                TMP_Text text = tmpTexts[i];
                if (i >= subtypes.Length)
                {
                    text.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    text.text = subtypes[i];
                }
            }

            // Put text into it
            for (int i = 0; i < subtypes.Length; i++)
            {
                // Set the purpose of each row
                mainRows[i].purposeSubtype = subtypes[i];

                // Find further details
                List<Blueprint> bps = Blueprint.SubtypeDictionary[subtypes[i]];
                Button[] buttons = mainRows[i].GetComponentsInChildren<Button>(true);
                for (int j = 0; j < buttons.Length; j++)
                {
                    // Set text
                    Text text = buttons[j].GetComponentInChildren<Text>(true);
                    if (text)
                    {
                        text.gameObject.SetActive(true);
                        text.text = bps[0].name;
                    }
                }
            }

            // Populate images in button
            for (int i = 0; i < mainRows.Count; i++)
            {
                Button[] buttons = mainRows[i].GetComponentsInChildren<Button>(true);
                for (int j = 0; j < buttons.Length; j++)
                {
                    // Set image
                    Image image = buttons[j].GetComponent<Image>();
                    image.color = Color.white / ((float)j);

                    // Just disable text
                    Text text = buttons[j].GetComponentInChildren<Text>();
                    if (text)
                        text.gameObject.SetActive(false);

                }
            }


            yield return new WaitForSeconds(0.1f);
        }

    }

    void UpdateLockedBlueprints()
    {
        // Go through each sub-types and start disabling.
        for (int i = 0; i < mainRows.Count; i++)
        {
            Button[] buttons = mainRows[i].GetComponentsInChildren<Button>(true);
            for (int j = 0; j < buttons.Length; j++)
            {
                // Set image
                Image image = buttons[j].GetComponent<Image>();
                image.color = Color.white / ((float)j);

                // Set text
                Text text = buttons[j].GetComponentInChildren<Text>();
                if (text)
                    text.gameObject.SetActive(false);
            }
        }
    }

    public void _TestRandomUnlock()
    {
        
        List<string> allBlueprines = new List<string>();
        foreach (string k in Blueprint.BlueprintsDictionary.Keys)
            allBlueprines.Add(k);

        int roll = Random.Range(0, allBlueprines.Count);
        PlayerData.Instance.UnlockBlueprint(allBlueprines[roll]);

        roll = Random.Range(0, allBlueprines.Count);
        PlayerData.Instance.UnlockBlueprint(allBlueprines[roll]);
        
    }

    public void _TestRandomLock()
    {        
        List<string> bpnames = new List<string>();
        foreach(Blueprint b in PlayerData.Instance.unlockedBlueprints)
            bpnames.Add(b.name);

        int roll = Random.Range(0, bpnames.Count);
        PlayerData.Instance._LockBlueprint(bpnames[roll]);        
    }

    void Update()
    {
        
    }
}
