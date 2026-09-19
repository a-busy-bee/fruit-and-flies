using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

using Random = UnityEngine.Random;

public class Object_Manager_Base : MonoBehaviour, IPointerClickHandler //TODO: split immune and not immune objects into two classes
{
    [SerializeField] protected Animator animator;
    private const int maxRerollsForRandomPoint = 50;

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

    public Vector3 GetRandomPointOnObj()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        Bounds objBounds = collider.bounds;
        float x = Random.Range(objBounds.min.x, objBounds.max.x);
        float y = Random.Range(objBounds.min.y, objBounds.max.y);

        for (int i = 0; i < maxRerollsForRandomPoint; i++)
        {
            Vector2 point = new Vector2(x, y);

            if (collider.OverlapPoint(point)) return point;
        }

        return objBounds.center;
    }
}
