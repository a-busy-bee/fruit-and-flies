using UnityEngine;
using UnityEngine.EventSystems;

public class Bug_Manager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Bug_Info bugInfo;
    private bool isDead;

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

    public float GetDamageDebuff()
    {
        return bugInfo.damageDebuff;
    }

    public void CheckDamage(GameObject collision)
    {
        if (collision == GetComponent<Steering_Controller>().GetTarget())
        {
            // do stuff
        }
    }

    #endregion

    public Bug_Info GetBugInfo()
    {
        return bugInfo;
    }

	public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("hit");
        // death

        if (!isDead)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();

            rb.mass = 2f;
            isDead = true;
        }


       
    }
}
