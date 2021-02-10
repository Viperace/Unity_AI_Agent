using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class SpawnPolygonButtons : MonoBehaviour
{
    [SerializeField] UIPolygon backgroundPolygon;
    [SerializeField] TMP_Text cornerTextPrefab;
    [SerializeField] GameObject cornerButtonPrefab;
    PolygonUIGame polygonGame;

    [SerializeField] float _radiusOffset = 100;
    [SerializeField] float _buttonYOffset = 50;
    [SerializeField] float _textYOffset = -50;
    float _radius;
    Vector2[] _cornerLocations;
    Button[] _buttons;
    TMP_Text[] _texts;
    void Start()
    {
        // Init
        _radius = (float) 0.5*backgroundPolygon.rectTransform.rect.width;
        polygonGame = GetComponentInParent<PolygonUIGame>();

    }

    Vector2[] FindCornerLocations(int N)
    {
        float angle = 360f / ((float)N) * Mathf.Deg2Rad;

        List<Vector2> pos = new List<Vector2>();
        for(int i = 0; i < N; i++)
        {
            float x = -(_radius + _radiusOffset) * Mathf.Cos(angle * i);
            float y = -(_radius + _radiusOffset) * Mathf.Sin(angle * i);
            pos.Add(new Vector2(x, y));
        }

        return pos.ToArray();
    }

    Button[] SpawnButtonsAroundCorners()
    {
        List<Button> buttons = new List<Button>();
        for(int i = 0; i < _cornerLocations.Length; i++)
        {
            Vector2 pos = _cornerLocations[i];

            // Create button and put into location
            GameObject buttonGO = Instantiate(cornerButtonPrefab, this.transform);
            buttonGO.GetComponent<RectTransform>().localPosition = pos + new Vector2(0, _buttonYOffset);

            // Attach function
            int x = i;
            buttonGO.GetComponent<Button>().onClick.AddListener(() => polygonGame._PushButton(x));
            buttons.Add(buttonGO.GetComponent<Button>());
        }
        return buttons.ToArray();
    }

    TMP_Text[] SpawnTextsAroundCorners()
    {
        List<TMP_Text> buttons = new List<TMP_Text>();
        for (int i = 0; i < _cornerLocations.Length; i++)
        {
            Vector2 pos = _cornerLocations[i];
            TMP_Text t = Instantiate<TMP_Text>(cornerTextPrefab, this.transform);
            t.GetComponent<RectTransform>().localPosition = pos + new Vector2(0, _textYOffset);

            // Set the button index
            PolygonCornerText cornerText = t.GetComponent<PolygonCornerText>();
            cornerText.SetCornerIndex(i);

            buttons.Add(t);
        }
        return buttons.ToArray();
    }

    TMP_Text[] SpawnTextsBelowButtons()
    {
        List<TMP_Text> texts = new List<TMP_Text>();
        for (int i = 0; i < _buttons.Length; i++)
        {
            Button btn = _buttons[i];
            if (btn != null)
            {
                TMP_Text t = Instantiate<TMP_Text>(cornerTextPrefab, btn.transform);
                t.GetComponent<RectTransform>().localPosition = new Vector2(0, _textYOffset);
                texts.Add(t);

                // Set the button index
                PolygonCornerText cornerText = t.GetComponent<PolygonCornerText>();
                cornerText.SetCornerIndex(i);

                // TODO: Cap the position of text to prevent out of bound
                //t.rectTransform.position = new Vector2(t.rectTransform.position.x, )
            }
        }
        return texts.ToArray();
    }

    public void SpawnButtonsForNewGame(int N)
    {
        // Remove old buttons
        if(_texts != null)
            foreach (TMP_Text t in _texts)
                Destroy(t.gameObject);

        if(_buttons != null)
            foreach (Button btn in _buttons)
                Destroy(btn.gameObject);

        // Instantiate new one
        _cornerLocations = FindCornerLocations(N);
        _buttons = SpawnButtonsAroundCorners();
        _texts = SpawnTextsBelowButtons();

    }
}
