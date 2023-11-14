using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyKillTracker : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    private int totalEnemiesDestroyed = 0;

    private void Start()
    {
        UpdateEnemyCountText();
    }

    public void EnemyDestroyed()
    {
        totalEnemiesDestroyed++;
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "Kills: " + totalEnemiesDestroyed.ToString();
        }
    }
}