using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGameScorer
{
    Polygon current;
    Polygon target;
    public PolygonGameScorer(Polygon current, Polygon target)
    {
        this.current = current;
        this.target = target;
    }

    int FulfilledCorners()
    {
        int count = 0;
        for(int i = 0; i < current.Nvertices; i++)
            if (current.hexagonValues[i] == target.hexagonValues[i])
                count++;

        return count;
    }

    int UnfilledCorners() => current.Nvertices - FulfilledCorners();

    /// <summary>
    /// Scoring system 1 to 10
    ///     Perfect = 10
    ///     1 miss  9
    ///     2 miss  8
    ///     3 miss  7
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        if (current != null)
        {
            int x = UnfilledCorners();
            if (x == 0)
                return 10;
            else if (x <= 9)
                return 10 - x;
            else
                return 0;
        }
        else
            return 0;
    }

    public bool AllFulfilled()
    {
        int count = 0;
        for (int i = 0; i < current.Nvertices; i++)
            if (current.hexagonValues[i] >= target.hexagonValues[i])
                count++;
        return count >= target.Nvertices;
    }
}

