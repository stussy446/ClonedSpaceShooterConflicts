using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#region Serializable classes
[System.Serializable]
public class EnemyWaves
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public Wave wave;
}

#endregion

public class LevelController : MonoBehaviour {

    //Serializable classes implements
    public EnemyWaves[] enemyWaves;

    public GameObject powerUp;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;

    [SerializeField] private TMP_Text remainingEnemiesText;

    private int remainingEnemies;

    List<GameObject> planetsList = new List<GameObject>();

    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        for (int i = 0; i<enemyWaves.Length; i++)
        {
            remainingEnemies += enemyWaves[i].wave.count;
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }

        StartCoroutine(PowerupBonusCreation());
        StartCoroutine(PlanetsCreation());

        remainingEnemiesText.text = remainingEnemies.ToString();
    }

    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, Wave wave)
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null)
        {
            var newWave = Instantiate(wave);
            newWave.enemySpawned += OnEnemySpawned;
        }
    }

    private void OnEnemySpawned()
    {
        remainingEnemies--;

        remainingEnemiesText.text = remainingEnemies.ToString();
    }

    //endless coroutine generating 'levelUp' bonuses.
    IEnumerator PowerupBonusCreation()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            Instantiate(
                powerUp,
                //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border
                new Vector2(
                    Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX),
                    mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2),
                Quaternion.identity
                );
        }
    }

    IEnumerator PlanetsCreation()
    {
        //Create a new list copying the arrey
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }
        yield return new WaitForSeconds(10);
        while (true)
        {
            ////choose random object from the list, generate and delete it
            int randomIndex = Random.Range(0, planetsList.Count);
            GameObject newPlanet = Instantiate(planetsList[randomIndex]);
            planetsList.RemoveAt(randomIndex);
            //if the list decreased to zero, reinstall it
            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }
            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;

            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }
}
