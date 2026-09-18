using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Object_Manager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Object_Info objectInfo;
    private Animator objectAnimator;
    private float currHealth;
    private bool isRotted; // threshold where SFX changes

    private void CheckRot(float damage)
    {
        currHealth -= damage;
    }

    #region Contact 
    public void OnPointerClick(PointerEventData data)
    {
        if (isRotted)
        {
            AudioManager.instance.PlaySound(objectInfo.soundTypeRotted, true);
            //Debug.Log("play " + objectInfo.soundTypeRotted);
        }
        else
        {
            AudioManager.instance.PlaySound(objectInfo.soundTypeNotRotted, true);
            //Debug.Log("play " + objectInfo.soundTypeNotRotted);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (objectInfo.immune) return;

        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug != null && bug.CausesDamage())
        {
            CheckRot(bug.GetInitialDamage());
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (objectInfo.immune) return;
        
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug != null && bug.CausesDamage())
        {
            CheckRot(bug.GetContinuousDamage());
        }
    }
    
    #endregion
}
