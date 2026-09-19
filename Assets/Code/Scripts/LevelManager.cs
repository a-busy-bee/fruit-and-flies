using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    [SerializeField] private GameObject[] objects;

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
}
