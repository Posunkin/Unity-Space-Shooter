using UnityEngine;

public class ScoreSystem : MonoBehaviour, IScoreObserver
{
    private int score = 0;

    private void Start()
    {
        GameManager.Instance.AddScoreObserver(this);
    }

    public void UpdateScore(int newScore)
    {
        score += newScore;
        if (score < 0) score = 0;
    }
}
