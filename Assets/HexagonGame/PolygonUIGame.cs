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

    PolygonGameScorer scorer;
    GameState gameState = GameState.RUNNING;

    public delegate void OnGameOver();
    public event OnGameOver onGameover; 

    public Polygon target { get; private set; }
    public Polygon current { get; private set; }
    int minValue = 2;

    public float score { get; private set; }


    /// <summary>
    /// This is to overcome the very odd bug. UIPolygon will NOT RESPECT set vertices at first load of new vertices. We need to wait for X frames or time, and reload it
    /// Thereafter it will respect,
    /// </summary>
    public void NewGame(int N = 6)
    {
        // Draw UI
        StartCoroutine(NewGameUnfix(N));
        StartCoroutine(NewGameUnfix(N, 5));

        // Spawn buttons
        SpawnPolygonButtons spawnButtons = GetComponentInChildren<SpawnPolygonButtons>();
        if (spawnButtons)
            spawnButtons.SpawnButtonsForNewGame(N);

        // Draw background lines
        DrawPolygonBackgroundLines draw = GetComponentInChildren<DrawPolygonBackgroundLines>();
        if (draw)
            draw.InitBackgroundLines(maxValue, N);

        // Set scorer
        scorer = new PolygonGameScorer(current, target);

        // Init
        gameState = GameState.RUNNING;
        onGameover = null;
    }
    IEnumerator NewGameUnfix(int N, float nFramesToSkip = 1)
    {
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
        for(int i = 0; i < nFramesToSkip; i++)
            yield return null;

        // Refresh
        backgroundHex.SetVerticesDirty();
        targetHex.SetVerticesDirty();
        currentHex.SetVerticesDirty();
        StartCoroutine(_ForcedUpdateUI(backgroundHex, Polygon.MaxValue(N, maxValue)));
        StartCoroutine(_ForcedUpdateUI(targetHex, target));
        StartCoroutine(_ForcedUpdateUI(currentHex, current));
    }

    public void QuitGame()
    {
        ExecuteEndGame();
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
        UpdateCurrentPolygon(currentHex, current);
    }

    void UpdateCurrentPolygon(UIPolygon hexUI, Polygon hexvalue)
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
        return scorer.GetScore();
    }

    void Update()
    {
        if (gameState == GameState.RUNNING)
        {
            // Check if game completed
            if(scorer != null)
                if (score == 10 || scorer.AllFulfilled()) 
                    ExecuteEndGame();
        }
    }

    void ExecuteEndGame()
    {
        gameState = GameState.END;

        // Calculate rewards

        // Do release control
        if (onGameover != null)
            onGameover();

        // Do 
        Debug.Log("HexGame completed. Your score is " + score);
    }

}

enum GameState
{
    RUNNING,
    END
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
