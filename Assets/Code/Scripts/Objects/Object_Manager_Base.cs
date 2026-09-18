using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Object_Manager_Base : MonoBehaviour, IPointerClickHandler //TODO: split immune and not immune objects into two classes
{
    [SerializeField] protected Animator animator;

    #region Contact 
    virtual public void OnPointerClick(PointerEventData data)
    {
        
    }

    virtual public void OnCollisionEnter2D(Collision2D collision)
    {
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug == null) return;

        TransitionBugToWalk(bug.gameObject);
    }

    virtual public void OnCollisionExit2D(Collision2D collision)
    {
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug == null) return;

        TransitionBugToFly(bug.gameObject);
    }

    protected void TransitionBugToWalk(GameObject bug)
    {
        // transition bug to walk when it lands (collides) on object
    }

    protected void TransitionBugToFly(GameObject bug)
    {
        // transition bug to fly when it leaves the object
    }

    #endregion

}
