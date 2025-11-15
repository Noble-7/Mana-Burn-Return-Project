using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //PLAYER STATS
    public int playerGold;
    public int playerMaxHealth;
    public int playerHealth;
    public float playerSpeed;
    public float playerSAttackDuration;
    public float playerProjectileSpeed;
    public float playerProjectileDamage;
    public float playerMeleeDamage;
    public float playerMAttackCooldownTime;
    public float playerSAttackCooldownTime;

    //DEFAULT STATS (FOR A FRESH SAVE FILE)
    private const int   defaultPlayerGold = 0;
    private const int   defaultPlayerMaxHealth = 10;
    private const int   defaultPlayerHealth = 10;
    private const float defaultPlayerSpeed = 5.0f;
    private const float defaultPlayerSAttackDuration = 0.1f;
    private const float defaultPlayerProjectileSpeed = 5.0f;
    private const float defaultPlayerProjectileDamage = 2.0f;
    private const float defaultPlayerMeleeDamage = 5.0f;
    private const float defaultPlayerMAttackCooldownTime = 1.0f;
    private const float defaultPlayerSAttackCooldownTime = 0.5f;

    public List<EnemyBehaviour> allEnemies = new List<EnemyBehaviour>();

    public enum gamestates {UIMENU,  GAMEPLAY, HUBWORLD};

    public gamestates currentGamestate;

    private void Awake()
    {
        switch (currentGamestate)
        {
            case gamestates.UIMENU:
            break;
            case gamestates.GAMEPLAY:
                //PLAYER BASE STATS (basically, change these if you come from a different menu)
                playerGold = defaultPlayerGold;
                playerMaxHealth = defaultPlayerMaxHealth;
                playerHealth = defaultPlayerHealth;              
                playerSpeed = defaultPlayerSpeed;              
                playerSAttackDuration = defaultPlayerSAttackDuration;               
                playerProjectileSpeed = defaultPlayerProjectileSpeed;               
                playerMeleeDamage = defaultPlayerMeleeDamage;               
                playerProjectileDamage = defaultPlayerProjectileDamage;
                playerMAttackCooldownTime = defaultPlayerMAttackCooldownTime;

                for(int i = 0; i < GameObject.FindObjectsOfType<EnemyBehaviour>().Length; i++)
                {
                    allEnemies.Add(GameObject.FindObjectsOfType<EnemyBehaviour>()[i]);
                }

            break;
            case gamestates.HUBWORLD:
            break;
        }
    }
}
