using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StashSlot : MonoBehaviour
{
    public TMP_Text headerText { get; private set; }
    public TMP_Text infoText { get; private set; }
    public TMP_Text bottomText { get; private set; }

    private void Start()
    {
        headerText = transform.Find("HeaderText").GetComponent<TMP_Text>();
        infoText = transform.Find("InfoText").GetComponent<TMP_Text>();
        bottomText = transform.Find("BottomText").GetComponent<TMP_Text>();
    }

    float _cooldown = 0;
    private void LateUpdate()
    {
        if(_cooldown < 0)
        {
            UpdateItemInfo();
            _cooldown = 1f;
        }
        _cooldown -= Time.deltaTime;
    }
    void UpdateItemInfo()
    {
        // Check if children exists
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            GameObject itemInSlot = meshRenderer.gameObject;

            BasicGear gear = StashUIItemsManager.Instance.GetDataFromUIgameObject(itemInSlot);
            Debug.Log("Gear found " + gear.name);

            headerText.text = gear.name;
            infoText.text = gear.attack.ToString();
            bottomText.text = "100g";
        }
        else
        {
            headerText.text = "";
            infoText.text = "";
            bottomText.text = "";
        }
    }

    public bool IsOccupied()
    {
        MeshRenderer m = GetComponentInChildren<MeshRenderer>();
        return m != null;
    }

    public void DestroyItem()
    {
        MeshRenderer m = GetComponentInChildren<MeshRenderer>();
        Destroy(m.gameObject);

        UpdateItemInfo();
    }

}
