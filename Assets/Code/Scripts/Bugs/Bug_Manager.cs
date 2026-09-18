using UnityEngine;

public class Bug_Manager : MonoBehaviour
{
    [SerializeField] private Bug_Info bugInfo;

    public bool CausesDamage()
    {
        return bugInfo.causesDamage;
    }
    public float GetInitialDamage()
    {
        return bugInfo.initialDamage;
    }

    public float GetContinuousDamage()
    {
        return bugInfo.continuousDamage;
    }
}
