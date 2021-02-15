using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateBlueprintUI : MonoBehaviour
{
    [SerializeField] RectTransform bodyPanel;
    List<Image> mainRows;
    List<TMP_Text> tmpTexts;
    
    void Start()
    {
        // Find all the relevant rows
        //Transform bodyPanel = transform.Find("BodyPanel");
        Image[] images = bodyPanel.GetComponentsInChildren<Image>(true);
        mainRows = new List<Image>();
        foreach(Image image in images)
            if (image.gameObject.name == "ItemRow")
                mainRows.Add(image);

        // For each rows, extract text
        tmpTexts = new List<TMP_Text>();
        foreach(Image image in mainRows)
        {
            TMP_Text text = image.GetComponentInChildren<TMP_Text>(true);
            tmpTexts.Add(text);
        }

        //Invoke("_LateInit", 0.5f);
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

            // Populate images in button
            for (int i = 0; i < mainRows.Count; i++)
            {
                Button[] buttons = mainRows[i].GetComponentsInChildren<Button>(true);
                for(int j = 0; j < buttons.Length; j++)
                {
                    // Set image
                    Image image = buttons[j].GetComponent<Image>();
                    image.color = Color.white / ((float) j);

                    // Set text
                    Text text = buttons[j].GetComponentInChildren<Text>();
                    if(text)
                        text.gameObject.SetActive(false);
                }
            }

            // Put text into it
            for (int i = 0; i < subtypes.Length; i++)
            {
                List<Blueprint> bps = Blueprint.GetBlueprintOfSubtypes(subtypes[i]);

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
            

            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        
    }
}
