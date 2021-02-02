using UnityEngine;

/* Stochastics and statistcs
*/
public static class MyStatistics
{
    public static int RandomWeightedIndex(float[] x)
    {
        if (x.Length == 1)
            return 0; // Only 1 value, no need to do

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
        return 0;
    }

	// Return lower matrix
	//https://www.geeksforgeeks.org/cholesky-decomposition-matrix-decomposition/
	public static float[,] Cholesky_Decomposition(float[,] matrix, int n)
	{
		float[,] lower = new float[n, n];

		// Decomposing a matrix
		// into Lower Triangular
		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j <= i; j++)
			{
				float sum = 0;

				// summation for diagnols
				if (j == i)
				{
					for (int k = 0; k < j; k++)
						sum += (float)System.Math.Pow(lower[j, k], 2);
					lower[j, j] = (float)System.Math.Sqrt(matrix[j, j] - sum);
				}

				else
				{
					// Evaluating L(i, j)
					// using L(j, j)
					for (int k = 0; k < j; k++)
						sum += (lower[i, k] * lower[j, k]);
					lower[i, j] = (matrix[i, j] - sum) / lower[j, j];
				}
			}
		}

		return lower;
	}

}

