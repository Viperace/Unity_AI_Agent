using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroInfoUI : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text midText;
    [SerializeField] TMP_Text bottomText;
    RectTransform panel;
    Actor actor;
    RolePlayingStatBehavior rpgStat;
    Inventory inventory;
    void Start()
    {
        panel = transform.Find("Panel").GetComponent<RectTransform>();
    }

    void SelectActor(GameObject gameObject)
    {
        if (gameObject)
            actor = gameObject.transform.root.GetComponent<Actor>();
        else
            actor = null;

        if (actor && actor.GetComponent<RolePlayingStatBehavior>())
            rpgStat = this.actor.GetComponent<RolePlayingStatBehavior>();
        else
            rpgStat = null;

        if (actor && actor.GetComponent<Inventory>())
            inventory = this.actor.GetComponent<Inventory>();
        else
            inventory = null;

    }

    void FillDisplay()
    {
        // Title
        if (rpgStat)
            titleText.text = string.Concat(rpgStat.Name, ", Lv", rpgStat.Level);            
        else
            titleText.text = "Not selected";

        // Middle
        if(inventory)
            midText.text = inventory.SummaryString();
        else
            midText.text = "-";

        // Bottom
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Player Click " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.CompareTag("Hero") || (hitInfo.transform.parent && hitInfo.transform.parent.CompareTag("Hero")))
                {
                    // Select this 
                    SelectActor(hitInfo.transform.gameObject);
                    FillDisplay();
                }
                else
                    SelectActor(null);
            }

            if (actor)
                panel.gameObject.SetActive(true);
            else
                panel.gameObject.SetActive(false);
        }
    }
}
