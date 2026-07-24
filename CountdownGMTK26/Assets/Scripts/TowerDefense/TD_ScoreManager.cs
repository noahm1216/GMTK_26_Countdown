using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TD_ScoreManager : MonoBehaviour
{

    public int lives = 20;
    public int currency = 100;
    public int score = 0;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI scoreText;

    public void LoseLife(int l =1) {
        lives -= l;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void ReduceCurrency(int amount)
    {
        currency -= amount;
    }

    public void GameOver()
    {
        // Handle game over logic, relaod for now
        Debug.Log("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        // Fix so text is not updated on each frame
        livesText.text = "Lives: " + lives.ToString();
        currencyText.text = "Currency: " + currency.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

}
