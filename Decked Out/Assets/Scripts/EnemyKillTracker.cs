using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyKillTracker : MonoBehaviour
{
    public TextMeshProUGUI enemyCountText;
    public int totalEnemiesDestroyed = 0;
    public float duration;
    private GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
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
            if (totalEnemiesDestroyed > 0)
            {
                StartCoroutine(ChangeTextColour(duration));
            }            
        }
    }
    
    IEnumerator ChangeTextColour(float duraton)
    {
        enemyCountText.color = Color.red;

        yield return new WaitForSeconds(duraton);

        enemyCountText.color = Color.white;
    }
}

  