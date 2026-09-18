using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Object_Manager_Rottable : Object_Manager_Base
{
    [SerializeField] private Object_Info_Rottable objectInfo;
    private float currHealth;

    public enum ObjectRotState
    {
        fresh,
        transitionToMold,
        moldy,
        tranisitionToRot,
        rotten,
        transitionToGone,
        gone
    }
    private ObjectRotState currRotState;
    private ObjectRotState prevRotState;

    private void Start()
    {
        currHealth = objectInfo.maxHealth;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        overrideController["ToMold"] = objectInfo.transitionToMold;
        overrideController["ToRot"] = objectInfo.transitionToRot;
        overrideController["ToGone"] = objectInfo.transitionToGone;
    }

    public void SetState(ObjectRotState newState)
    {
        prevRotState = currRotState;
        currRotState = newState;

        switch (currRotState)
        {
            case ObjectRotState.fresh:
                break;
            case ObjectRotState.transitionToMold:
                animator.Play("ToMold");
                StartCoroutine(WaitForAnimEnd_ThenTransitionState());

                break;
            case ObjectRotState.moldy:

                break;
            case ObjectRotState.tranisitionToRot:
                animator.Play("ToRot");
                StartCoroutine(WaitForAnimEnd_ThenTransitionState());

                break;
            case ObjectRotState.rotten:

                break;
            case ObjectRotState.transitionToGone:
                animator.Play("ToGone");
                StartCoroutine(WaitForAnimEnd_ThenTransitionState());

                break;
            case ObjectRotState.gone:

                break;
        }
    }

    private void CheckRot(float damage)
    {
        if (currRotState == ObjectRotState.transitionToMold ||
            currRotState == ObjectRotState.tranisitionToRot ||
            currRotState == ObjectRotState.transitionToGone) return; // if transitioning anim, don't take damage

        currHealth -= damage;

        // technically this could be cleaner but this way it's more readable
        if (currRotState == ObjectRotState.fresh && currHealth <= 0.7f * objectInfo.maxHealth)
        {
            SetState(ObjectRotState.transitionToMold);
        }
        else if (currRotState == ObjectRotState.moldy && currHealth <= 0.4f * objectInfo.maxHealth)
        {
            SetState(ObjectRotState.tranisitionToRot);
        }
        else if (currRotState == ObjectRotState.rotten && currHealth <= 0.1 * objectInfo.maxHealth)
        {
            SetState(ObjectRotState.transitionToGone);
        }
    }

    #region Contact 
    override public void OnPointerClick(PointerEventData data)
    {
        if (currRotState == ObjectRotState.rotten)
        {
            AudioManager.instance.PlaySound(objectInfo.soundTypeRotted, true);
            //Debug.Log("play " + objectInfo.soundTypeRotted);
        }
        else
        {
            AudioManager.instance.PlaySound(objectInfo.soundTypeDefault, true);
            //Debug.Log("play " + objectInfo.soundTypeNotRotted);
        }
    }

    override public void OnCollisionEnter2D(Collision2D collision)
    {
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug == null) return;

        TransitionBugToWalk(bug.gameObject);
        if (bug.CausesDamage())
        {
            CheckRot(bug.GetInitialDamage());
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug == null) return; 

        if (bug.CausesDamage())
        {
            CheckRot(bug.GetContinuousDamage());
        }
    }

    override public void OnCollisionExit2D(Collision2D collision)
    {
        Bug_Manager bug = collision.gameObject.GetComponent<Bug_Manager>();
        if (bug == null) return;

        TransitionBugToFly(bug.gameObject);
    }

    #endregion

    #region Helpers & IEnumerators
    private IEnumerator WaitForAnimEnd_ThenTransitionState()
    {
        yield return new WaitForSeconds(1);

        SetState(currRotState + 1);
    }
    
    #endregion
}
