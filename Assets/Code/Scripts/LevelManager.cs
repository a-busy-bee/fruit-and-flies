using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    

    // FADE TO BLACK
    [SerializeField] private GameObject blackbkg;
    [SerializeField] private CanvasGroup fadeToBlack;
    private float currVelocity;
    bool fading = false;
    bool lose = false;

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

    }

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
        fading = true;
        lose = isLose;
    }

    private void Update()
    {
        if (fading)
        {
            fadeToBlack.alpha = Mathf.SmoothDamp(fadeToBlack.alpha, 1, ref currVelocity, 0.75f);

            if ((1 - fadeToBlack.alpha) < 0.0001f)
            {
                fading = false;

                if (lose) 
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                else 
                    SceneManager.LoadScene("Menu");
            }
        }
    }

    #endregion
}
