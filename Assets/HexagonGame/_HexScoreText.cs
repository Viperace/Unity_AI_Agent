using UnityEngine;
using UnityEngine.UI;

public class _HexScoreText : MonoBehaviour
{
    [SerializeField] PolygonUIGame polygonGame;
    Text scoreText;

    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (polygonGame)
        {
            scoreText.text = Mathf.RoundToInt(polygonGame.score * 100).ToString();
        }
        else
            scoreText.text = "-";
    }
}
