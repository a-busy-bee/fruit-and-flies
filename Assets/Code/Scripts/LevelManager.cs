using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    [SerializeField] private const float LOSE_THRESHOLD = 0.3f;
    [SerializeField] private GameObject[] objects;
    private List<Object_Manager_Rottable> rottableObjects = new List<Object_Manager_Rottable>();
    private List<Vector3> currTargets; 
    private float globalRotState = 0f;
    private float globalMaxHealth = 0f;
    private float globalCurrHealth = 0f;
    

    // SPAWNING FLIES
    [SerializeField] private Transform spawnLeft;
    [SerializeField] private Transform spawnRight;
    [SerializeField] private GameObject prefabBigFly;
    [SerializeField] private GameObject prefabFruitFly;
    private const float maxFlyMultiplier = 1.3f; // multiplier because it affects the formula (which is constant)
    private int maxFlies = 2; 
    private int currNumFlies = 0;


    // FADE TO BLACK
    [SerializeField] private GameObject blackbkg;
    [SerializeField] private Animator fadeToBlack;
    private float currVelocity;
    bool levelRunning = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        // MUSIC
        string sceneName = SceneManager.GetActiveScene().name;

        AudioManager.MusicType theme = AudioManager.MusicType.baseTheme;

        switch (sceneName)
        {
            case "Tutorial":
                theme = AudioManager.MusicType.tutorial;
                break;
            case "Bargaining":
                theme = AudioManager.MusicType.breakfast;
                break;
            case "Depression":
                theme = AudioManager.MusicType.dinner;
                break;
            case "Acceptance":
                theme = AudioManager.MusicType.midnight;
                break;
        }

        AudioManager.instance.SwitchTheme(theme);

        // LEVEL STATE
        foreach (GameObject obj in objects)
        {
            if (obj.TryGetComponent<Object_Manager_Rottable>(out Object_Manager_Rottable rotObj))
            {
                rottableObjects.Add(rotObj);
                globalMaxHealth += rotObj.GetMaxHealth();
            }
        }

        globalRotState = 0f;
        globalCurrHealth = globalMaxHealth;

        // SPAWNING
        // spawn first batch from the right
        while (currNumFlies < maxFlies)
        {
            Spawn(false);
        }

        StartCoroutine(IncreaseMaxFlies());
    }

    #region SPAWNING FLIES 

    public void Spawn(bool isLeft)
    {
        if (levelRunning) return;
        float chance = Random.Range(0.0f, 1.0f);
        Transform spawnTarget = isLeft ? spawnLeft : spawnRight; // (condition) ? if true : if false
        GameObject prefab = (chance < 0.35f) ? prefabBigFly : prefabFruitFly;

        Instantiate(prefab, spawnTarget);
        currNumFlies++;
    }

    public void KillFly()
    {
        currNumFlies--;

        if (currNumFlies <= 0.5f * maxFlies)
        {
            StartCoroutine(SpawnFlies());
        }
    }

    private IEnumerator IncreaseMaxFlies()
    {
        while (!levelRunning)
        {
            yield return new WaitForSeconds(10);

            maxFlies = (int)(maxFlies * maxFlyMultiplier);

            SpawnFlies();
        }
    }

    private IEnumerator SpawnFlies()
    {
        while (currNumFlies < maxFlies)
        {
            float chance = Random.Range(0.0f, 1.0f);
            bool isLeft = (chance < 0.5f) ? true : false;
            Spawn(isLeft);

            yield return new WaitForSeconds(Random.Range(0, 3));
        }
    }


    #endregion

    #region OBJECTS
    public GameObject GetRandomObject()
    {
        return objects[Random.Range(0, objects.Length)];
    }

    public Vector3 GetRandomPointInAir()
    {
        return new Vector3(Random.Range(-20, 20), Random.Range(0, 10), 0);
    }

    public void ReportNewRot(float damage)
    {
        globalCurrHealth -= damage;

        globalRotState = globalCurrHealth / globalMaxHealth;

        if (globalRotState <= LOSE_THRESHOLD)
        {
            FadeToBlack(true);
        }
    }

    #endregion


    #region FADE TO BLACK

    public void FadeToBlack(bool isLose)
    {
        StartCoroutine(FtbCoroutine(isLose));
    }

    IEnumerator FtbCoroutine(bool isLose)
    {
        fadeToBlack.SetTrigger("Close");
        levelRunning = true;
        yield return new WaitForSeconds(3f);
        if (isLose) 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else 
            SceneManager.LoadScene("Menu");
    }

    #endregion
}
