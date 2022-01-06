using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject destructionFX;
    [SerializeField] private int health = 3;

    public static Player instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destruction();
    }

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        Destroy(gameObject);
    }
}
















