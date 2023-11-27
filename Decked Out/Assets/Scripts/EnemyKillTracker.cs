using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyKillTracker : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI endGameEnemyCountText;
    public TextMeshProUGUI endGameWave;
    public int totalEnemiesDestroyed = 0;
    public int currentWave = 1;
    public float duration;
    private GameLoader _loader;
    private WaveManager mouse;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        UpdateEnemyCountText();
        mouse = ServiceLocator.Get<WaveManager>();
    }

    public void EnemyDestroyed()
    {
        totalEnemiesDestroyed++;
        UpdateEnemyCountText();
    }

    public void WaveUpdate()
    {
        currentWave++;
        UpdateEnemyCountText();
    }

    public void resetWave()
    {
        currentWave = 1;
        UpdateEnemyCountText();
    }

    public void ResetValues()
    {
        totalEnemiesDestroyed = 0;
        currentWave = 1;
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString();
            if (totalEnemiesDestroyed > 0)
            {
                StartCoroutine(ChangeTextColour(duration));
            }
        }
        wave.text = "Wave: " + currentWave.ToString();
    }

    IEnumerator ChangeTextColour(float duration)
    {
        enemyCountText.color = Color.red;

        yield return new WaitForSeconds(duration);

        enemyCountText.color = Color.white;
    }

    public void EndGame()
    {
        endGameEnemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString();
        endGameWave.text = "Wave: " + currentWave.ToString();

        // Reset values when the game ends
        ResetValues();
    }
}

