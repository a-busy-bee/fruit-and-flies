using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    [SerializeField] private GameObject[] objects;

    private List<Vector3> currTargets; 

    public GameObject GetRandomObject()
    {
        return objects[Random.Range(0, objects.Length)];
    }

    public Vector3 GetRandomPointInAir()
    {
        return new Vector3(Random.Range(-20, 20), Random.Range(0, 10), 0);
    }

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
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

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

    }
}
