using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PolygonCornerText : MonoBehaviour
{
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
    void LateUpdate()
    {
        if(polygonUIGame)
        {
            _curr = polygonUIGame.current.hexagonValues[cornerIndex];
            _target = polygonUIGame.target.hexagonValues[cornerIndex];
            text.text = string.Concat(_curr, "/", _target);
        }
    }
}
