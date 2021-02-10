using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PolygonCornerText : MonoBehaviour
{
    [SerializeField] Color goodColor;
    [SerializeField] Color badColor;
    [SerializeField] Color stillGoingColor;
    PolygonUIGame polygonUIGame;
    TMP_Text text;
    int cornerIndex;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        polygonUIGame = GetComponentInParent<PolygonUIGame>();
    }

    public void SetCornerIndex(int i)
    {
        cornerIndex = i;
    }

    int _curr, _target;
    string _temptext;
    void LateUpdate()
    {
        if(polygonUIGame)
        {
            _curr = polygonUIGame.current.hexagonValues[cornerIndex];
            _target = polygonUIGame.target.hexagonValues[cornerIndex];
            _temptext = string.Concat(_curr, "/", _target);

            if (_curr > _target)
                text.SetText($"{_temptext.AddColor(badColor)}");
            else if (_curr == _target)
                text.SetText($"{_temptext.AddColor(goodColor)}");
            else
                text.SetText($"{_temptext.AddColor(stillGoingColor)}");
        }
    }
}

public static class StringExtensions
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}