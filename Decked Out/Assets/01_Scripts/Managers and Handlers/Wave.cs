using System;
using System.Collections.Generic;

[Serializable]
public class Wave
{
    public int numberOfEnemies = 5;
    public float timeBetweenEnemies = 2.0f;
    public float timeBetweenWaves = 10.0f;
    public SerializableDictionary<string, float> enemySpawnPercentages = new SerializableDictionary<string, float>
    {
        { "Acolyte", 0.5f },
        { "Kaboom", 0.1f },
        { "Golem", 0.1f },
        { "Apostate", 0.1f },
        { "Necromancer", 0.1f },
        { "Aegis", 0.05f },
        { "Cleric", 0.05f }
    };
}
