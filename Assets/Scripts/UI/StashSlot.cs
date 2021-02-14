using UnityEngine;
using TMPro;
public class StashSlot : MonoBehaviour
{
    public TMP_Text headerText { get; private set; }
    public TMP_Text infoText { get; private set; }
    public TMP_Text bottomText { get; private set; }

    BasicGear gear;
    GearTextDisplay gearTextDisplay;
    private void Start()
    {
        headerText = transform.Find("HeaderText").GetComponent<TMP_Text>();
        infoText = transform.Find("InfoText").GetComponent<TMP_Text>();
        bottomText = transform.Find("BottomText").GetComponent<TMP_Text>();
    }

    float _cooldown = 0;
    float _updatePeriod = 0.25f;
    private void LateUpdate()
    {
        if(_cooldown < 0)
        {
            UpdateItemInfo();
            _cooldown = _updatePeriod;
        }
        _cooldown -= Time.deltaTime;
    }
    void UpdateItemInfo()
    {
        // Check if children exists by checking DragTransform Component
        RotateIfOnUI dragTransform = GetComponentInChildren<RotateIfOnUI>();
        if (dragTransform)
        {
            GameObject itemInSlot = dragTransform.gameObject;

            gear = StashUIItemsManager.Instance.GetDataFromUIgameObject(itemInSlot);
            gearTextDisplay = new GearTextDisplay(gear);

            headerText.text = gearTextDisplay.ColoredName();
            infoText.text = gearTextDisplay.ColoredStat();
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
