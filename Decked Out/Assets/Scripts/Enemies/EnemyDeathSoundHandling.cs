using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathSoundHandling : MonoBehaviour
{
    public AudioClip enemyDeathSound;

    public void PlayDeathSound()
    {
        GameObject temp = new GameObject("Death Sound Player");
        AudioSource audioSource = temp.AddComponent<AudioSource>();
        audioSource.clip = enemyDeathSound;
        audioSource.Play();
        Destroy(temp, enemyDeathSound.length);
    }
}
