using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class DrawPolygonBackgroundLines : MonoBehaviour
{
    [SerializeField] GameObject uiPolygonPrefab;
    [SerializeField] UIPolygon background;


    public void InitBackgroundLines(int numberOfLevel, int polygonVertices)
    {
        StartCoroutine(InitBackgroundLinesCoroutine(numberOfLevel, polygonVertices));
        StartCoroutine(InitBackgroundLinesCoroutine(numberOfLevel, polygonVertices, 5));
    }


    IEnumerator InitBackgroundLinesCoroutine(int numberOfLevel, int polygonVertices, int nFramesToSkip = 1)
    {
        // Wait for N frames
        for (int i = 0; i < nFramesToSkip; i++)
            yield return null;

        // Find existing polygon
        UIPolygon[] uiPolygons = GetComponentsInChildren<UIPolygon>(true);

        // Instatiate new one, if not enough , or...
        List<UIPolygon> allPolygons = new List<UIPolygon>();
        for (int i = 0; i < numberOfLevel - 1; i++)
        {
            if (uiPolygons.Length > i)
            {
                uiPolygons[i].gameObject.SetActive(true);
                allPolygons.Add(uiPolygons[i]);
            }
            else
            {
                GameObject go = Instantiate(uiPolygonPrefab, this.transform);
                allPolygons.Add(go.GetComponent<UIPolygon>());
            }
        }

        // ... Deactivate if too many
        uiPolygons = GetComponentsInChildren<UIPolygon>(true);
        for (int i = 0; i < uiPolygons.Length; i++)
            if (i >= numberOfLevel - 1)
                uiPolygons[i].gameObject.SetActive(false);

        // For each polygon, draw
        float width = background.rectTransform.rect.width;
        float height = background.rectTransform.rect.height;
        for (int i = 0; i < allPolygons.Count; i++)
            if (i < uiPolygons.Length)
            {
                allPolygons[i].SetAllDirty();
                allPolygons[i].sides = polygonVertices;
                allPolygons[i].rectTransform.sizeDelta = new Vector2(width / numberOfLevel, height / numberOfLevel) * (i + 1);
            }
    }

}
