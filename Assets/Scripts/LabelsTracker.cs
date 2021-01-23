using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelsTracker : MonoBehaviour
{
    TMP_Text[] labelTexts;
    List<ViewedByCamera> herosWithinView;
    Dictionary<ViewedByCamera, TMP_Text> actorPositionDict;
    public float refreshPeriod = 1f;
    float _refreshCooldown = 1f;
    public float offsetY = -2f;
    void Awake()
    {
        actorPositionDict = new Dictionary<ViewedByCamera, TMP_Text>();
        labelTexts = GetComponentsInChildren<TMP_Text>(true);

        // Init
        herosWithinView = new List<ViewedByCamera>();
        actorPositionDict = new Dictionary<ViewedByCamera, TMP_Text>();

        // 
        UpdateAllHerosWithinView();
    }

    void UpdateAllHerosWithinView()
    {
        // Find all heros
        herosWithinView.Clear();
        foreach (ViewedByCamera a in FindObjectsOfType<ViewedByCamera>())
            if (a.transform.parent.CompareTag("Hero") && a.IsVisible)
                herosWithinView.Add(a);
    }

    void TagTargetsWithLabelTexts()
    {
        actorPositionDict.Clear();
        int nlabel = Mathf.Min(herosWithinView.Count, labelTexts.Length);
        for (int i = 0; i < labelTexts.Length; i++) 
        {
            if (i < herosWithinView.Count)
            {
                actorPositionDict.Add(herosWithinView[i], labelTexts[i]);
                labelTexts[i].gameObject.SetActive(true);
                labelTexts[i].text = GetHighLevelPlanText(herosWithinView[i]);
            }
            else
                labelTexts[i].gameObject.SetActive(false);
        }
    }

    string GetHighLevelPlanText(ViewedByCamera v)
    {
        Brain brain = v.GetComponentInParent<Brain>();
        if (brain != null)
            return brain.CurrentPlan.ToString();
        else
            return "UNKNOWN";
    }

    Vector3 namePos;
    void Update()
    {
        // Refresh hero to load
        _refreshCooldown -= Time.deltaTime;
        if(_refreshCooldown < 0)
        {
            _refreshCooldown = refreshPeriod;
            UpdateAllHerosWithinView();
            TagTargetsWithLabelTexts();
        }

        // Update their movement
        if(herosWithinView != null)
            foreach(ViewedByCamera v in herosWithinView)
                if (v)
                {
                    namePos = Camera.main.WorldToScreenPoint(v.transform.position - Vector3.down * offsetY);
                    actorPositionDict[v].transform.position = namePos;
                }
    }
}
