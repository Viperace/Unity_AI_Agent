using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
public class PolygonUIGame : MonoBehaviour
{
    List<Button> buttons;
    [SerializeField] UIPolygon targetHex;
    [SerializeField] UIPolygon currentHex;
    [SerializeField] UIPolygon backgroundHex;
    [SerializeField] float triggerPercentage = 0.1f;
    [SerializeField] int maxValue = 6;
    public Polygon target { get; private set; }
    public Polygon current { get; private set; }
    int minValue = 1;
    int _currentNvertices;

    public float score { get; private set; }


    /// <summary>
    /// This is to overcome the very odd bug. UIPolygon will NOT RESPECT set vertices at first load of new vertices. We need to wait for X frames or time, and reload it
    /// Thereafter it will respect,
    /// </summary>
    public void NewGame(int N = 6)
    {
        StartCoroutine(NewGameUnfix(N));
        StartCoroutine(NewGameUnfix(N, 5));

        SpawnPolygonButtons spawnButtons = GetComponentInChildren<SpawnPolygonButtons>();
        if (spawnButtons)
            spawnButtons.SpawnButtonsForNewGame(N);
    }
    IEnumerator NewGameUnfix(int N, float nFrame=1)
    {
        _currentNvertices = N;
        
        // Randomize hex to solve        
        target = new Polygon(N);
        for (int i = 0; i < N; i++)
            target.hexagonValues[i] = Random.Range(minValue, maxValue + 1);

        // Randomize Initial
        current = new Polygon(N);
        for (int i = 0; i < N; i++)
            current.hexagonValues[i] = Mathf.Min(Random.Range(1, 3), target.hexagonValues[i]);

        // Set polygon size
        targetHex.sides = N;
        currentHex.sides = N;
        backgroundHex.sides = N;

        // Wait for N frames
        for(int i = 0; i < nFrame; i++)
            yield return null;

        // Refresh
        backgroundHex.SetVerticesDirty();
        targetHex.SetVerticesDirty();
        currentHex.SetVerticesDirty();
        StartCoroutine(_ForcedUpdateUI(backgroundHex, Polygon.MaxValue(N, maxValue)));
        StartCoroutine(_ForcedUpdateUI(targetHex, target));
        StartCoroutine(_ForcedUpdateUI(currentHex, current));
    }

    public void _PushButton(int i)
    {
        if (current != null && current.hexagonValues.Length >= i)
        {
            current.hexagonValues[i]++;
            current.hexagonValues[i] = Mathf.Min(maxValue, current.hexagonValues[i]);

            if (Random.value < triggerPercentage)
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
        _TestdUpdateUI(currentHex, current);
    }

    void _TestdUpdateUI(UIPolygon hexUI, Polygon hexvalue)
    {
        hexUI.SetVerticesDirty();

        // Update UI Value
        if (hexvalue != null)
            for (int i = 0; i < hexvalue.Nvertices; i++)
                hexUI.VerticesDistances[i] = hexvalue.hexagonValues[i] / ((float)maxValue);
    }

    IEnumerator _ForcedUpdateUI(UIPolygon hexUI, Polygon hexvalue)
    {
        yield return null; // Must give time to let the vertices update

        // Update UI Value
        if (hexvalue != null)
            for (int i = 0; i < hexvalue.Nvertices; i++)
                hexUI.VerticesDistances[i] = hexvalue.hexagonValues[i] / ((float)maxValue);

        yield return null;
    }

    // The lowest the better
    float GetScore()
    {
        if (current != null)
        {
            int n = current.hexagonValues.Length;
            float deviation = 0;
            for (int i = 0; i < n; i++)
                deviation += Mathf.Abs(current.hexagonValues[i] - target.hexagonValues[i]);

            float score = 1f - deviation / ((float) maxValue * n);

            return score;
        }
        else
            return 0f;
    }
}



public class Polygon
{
    // Start with topright
    public int[] hexagonValues { get; private set; }
    public int Nvertices { get; private set; }
    public Polygon(int N) 
    {
        hexagonValues = new int[N];
        for (int i = 0; i < hexagonValues.Length; i++)
            hexagonValues[i] = 1;

        Nvertices = N;
    }

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

    public static Polygon MaxValue(int N, int maxValue)
    {
        Polygon p = new Polygon(N);
        for (int i = 0; i < p.Nvertices; i++)
            p.hexagonValues[i] = maxValue;
        return p;
    }
}