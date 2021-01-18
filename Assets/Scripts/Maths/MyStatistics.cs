using UnityEngine;

/* Stochastics and statistcs
*/
public static class MyStatistics
{
    public static int RandomWeightedIndex(float[] x)
    {
        // Find cumsum
        float[] xcum = new float[x.Length];
        for (int i = 0; i < x.Length; i++)
            if (i == 0)
                xcum[i] = x[i];
            else
                xcum[i] = xcum[i - 1] + x[i];


        // Roll
        float rollVal = Random.Range(0, xcum[xcum.Length - 1]);

        // Decide which bucket it falls in.
        for (int i = 1; i < x.Length; i++)
            if (rollVal > xcum[i - 1] && rollVal <= xcum[i])
                return i;

        return -1; // Error, cannot be 
    }
}

