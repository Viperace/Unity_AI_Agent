using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NeedsBar : MonoBehaviour
{
    [SerializeField] TMP_Text bloodLustText;
    [SerializeField] TMP_Text HP_Text;
    [SerializeField] TMP_Text energy_Text;
    [SerializeField] TMP_Text shopping_Text;

    NeedsBehavior needsBehavior;
    Transform parent;
    Canvas canvas;

    void Start()
    {
        needsBehavior = GetComponentInParent<NeedsBehavior>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        parent = needsBehavior.transform;
        
    }

    void LateUpdate()
    {
        if (needsBehavior)
        {
            bloodLustText.text = BeautifyText(needsBehavior.needs.BloodLust);
            HP_Text.text = BeautifyText(needsBehavior.needs.HP);
            energy_Text.text = BeautifyText(needsBehavior.needs.Energy);
            shopping_Text.text = BeautifyText(needsBehavior.needs.Shopping);

            AdjustColor(bloodLustText, needsBehavior.needs.BloodLust);
            AdjustColor(HP_Text, needsBehavior.needs.HP);
            AdjustColor(energy_Text, needsBehavior.needs.Energy);
            AdjustColor(shopping_Text, needsBehavior.needs.Shopping);
        }

        // LookAt camera
        //transform.LookAt(Camera.main.transform);
        transform.SetPositionAndRotation(this.transform.position,
            Quaternion.Euler(0, - parent.rotation.y + canvas.worldCamera.transform.rotation.eulerAngles.y, 0));
    }

    string BeautifyText(float x) => Mathf.RoundToInt(x).ToString();
    
    // Adjust color to Red/yellow/ White based on level
    void AdjustColor(TMP_Text tmText, float value)
    {
        if (value >= 70)
            tmText.color = Color.green;
        else if (value >= 30)
            tmText.color = Color.yellow;
        else
            tmText.color = Color.red;
    }
}
