using UnityEngine;

public class Bug_Manager : MonoBehaviour
{
    [SerializeField] private Bug_Info bugInfo;

    #region Damage
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

    #endregion

    public Bug_Info GetBugInfo()
    {
        return bugInfo;
    }
}
