using UnityEngine;
using System.Collections.Generic;

public class ConsonantLetterManager : MonoBehaviour
{
    public static ConsonantLetterManager instance;
    private List<ConsonantEnemy> allConsonantEnemies = new List<ConsonantEnemy>(); // Keep track of all consonant enemies
    private PlayerHealth player;
    private ConsonantEnemy enemy;// Reference to PlayerHealth

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterEnemy(ConsonantEnemy enemy)
    {
        allConsonantEnemies.Add(enemy);
    }

    public void UnregisterEnemy(ConsonantEnemy enemy)
    {
        allConsonantEnemies.Remove(enemy);
    }

}