using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class HexogonUIGame : MonoBehaviour
{    
    [SerializeField] Button upRightBotton;
    [SerializeField] Button rightBotton;
    [SerializeField] Button downRightBotton;
    [SerializeField] Button downLeftBotton;
    [SerializeField] Button leftBotton;
    [SerializeField] Button upLeftBotton;

    [SerializeField] UIPolygon targetHex;
    [SerializeField] UIPolygon currentHex;
    //[SerializeField] UIPolygon background;
    [SerializeField] TMP_Text[] labelTexts;
    [SerializeField] float triggerPercentage = 0.1f;
    Hexagon target;
    Hexagon current;
    int maxValue = 6;
    int minValue = 1;
    public float score { get; private set; }

    void Start()
    {
        Invoke("NewGame", 0.1f);
    }

    void LateUpdate()
    {
        if(current != null)
            for(int i = 0; i < labelTexts.Length; i++)
            {
                labelTexts[i].text = string.Concat(current.hexagonValues[i],"/6");
            }
    }

    public void NewGame()
    {
        
        // Randomize hex to solve        
        target = new Hexagon();
        for(int i = 0; i< 6; i++)
            target.hexagonValues[i] = Random.Range(minValue, maxValue + 1);

        // Randomize Initial
        current = new Hexagon();
        for (int i = 0; i < 6; i++)
            current.hexagonValues[i] = Mathf.Min(Random.Range(1, 3),
                target.hexagonValues[i]);

        // Refresh
        StartCoroutine(_ForcedUpdateUI(targetHex, target));
        StartCoroutine(_ForcedUpdateUI(currentHex, current));
    }

    public void _PushButton(int i)
    {
        if (current != null && current.hexagonValues.Length >= i) 
        { 
            current.hexagonValues[i]++;
            current.hexagonValues[i] = Mathf.Min(maxValue, current.hexagonValues[i]);

            if(Random.value < triggerPercentage)
            {
                int left = current.GetLeftNeighbor(i);
                int right = current.GettRightNeighbor(i);

                current.hexagonValues[left]++;
                current.hexagonValues[left] = Mathf.Min(maxValue, current.hexagonValues[left]);

                current.hexagonValues[right]++;
                current.hexagonValues[right] = Mathf.Min(maxValue, current.hexagonValues[right]);
            }
        }

        // Update score
        this.score = GetScore();

        // Update UI
        StartCoroutine(_ForcedUpdateUI(currentHex, current));
    }

    IEnumerator _ForcedUpdateUI(UIPolygon hexUI, Hexagon hexvalue)
    {
        // Update UI Value
        if (hexvalue != null)
            for (int i = 0; i < 6; i++)
                hexUI.VerticesDistances[i] = hexvalue.hexagonValues[i] / ((float)maxValue); ;

        hexUI.enabled = false;
        yield return null;
        hexUI.enabled = true;
    }

    IEnumerator _ForcedUpdateUI()
    {
        if (current != null & target != null)
        {
            // Update UI Value
            for (int i = 0; i < 6; i++)
            {
                targetHex.VerticesDistances[i] = target.hexagonValues[i] / ((float)maxValue); ;
                currentHex.VerticesDistances[i] = current.hexagonValues[i] / ((float)maxValue); ;
            }
        }

        targetHex.enabled = false;
        currentHex.enabled = false;
        yield return null;
        targetHex.enabled = true;
        currentHex.enabled = true;
        

    }

    // The lowest the better
    float GetScore()
    {
        if (current != null)
        {
            int n = current.hexagonValues.Length;
            float score = 0;
            for (int i = 0; i < n; i++)
                score += current.hexagonValues[i] - target.hexagonValues[i];

            //score /= n;
            return score;
        }
        else
            return -1;
    }
}

class Hexagon
{
    // Start with topright
    public int[] hexagonValues = new int[6] { 1, 1, 1, 1, 1, 1 };
    public Hexagon(){} 

    public int GetLeftNeighbor(int i)
    {
        int x = i - 1;
        if (x < 0)
            return hexagonValues.Length - 1;
        return x;
    }

    public int GettRightNeighbor(int i)
    {
        int x = i + 1;
        if (x == hexagonValues.Length)
            return 0;
        return x;
    }
}