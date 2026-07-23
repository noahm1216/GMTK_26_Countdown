using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private float score;
    private int highScore;

    public int CurrentScore => Mathf.FloorToInt(score);
    public int HighScore => highScore;

    private bool isCounting;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        UpdateUI();
    }

    private void Update()
    {
        if (!isCounting)
            return;

        score += Time.deltaTime;

        UpdateUI();
    }

    public void StartScore()
    {
        score = 0;
        isCounting = true;

        UpdateUI();
    }

    public void StopScore()
    {
        isCounting = false;

        CheckHighScore();
        UpdateUI();
    }

    private void CheckHighScore()
    {
        if (CurrentScore > highScore)
        {
            highScore = CurrentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {CurrentScore}";

        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScore}";
    }
}