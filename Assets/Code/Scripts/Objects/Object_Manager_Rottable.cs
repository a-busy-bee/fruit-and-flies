using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Object_Manager_Rottable : Object_Manager_Base
{
    [SerializeField] private Object_Info_Rottable objectInfo;
    [SerializeField] private Mold[] moldsStage1;
    [SerializeField] private Mold[] moldsStage2;
    private float currHealth;
    private float damageDebuff;

    public enum ObjectRotState
    {
        fresh,
        transitionToMold,   // mold appear anim (no contact, spawn mold)
        moldy,              // mold stays half rot (contact allowed)
        tranisitionToRot,   // final mold appear anim (no contact, more mold spawns, anim transition to final rot)
        rotten              // final sprite (no movement, no contact, for now replace with temp rot sprite)
    }
    private ObjectRotState currRotState;
    private ObjectRotState prevRotState;

    private void Start()
    {
        currHealth = objectInfo.maxHealth;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(rotAnimator.runtimeAnimatorController);
        rotAnimator.runtimeAnimatorController = overrideController;
        overrideController["ToRot"] = objectInfo.transitionToRot;

        //rotAnimator.enabled = false;
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
                
                foreach (Mold mold in moldsStage1)
                {
                    mold.GrowMold();
                }

                StartCoroutine(MoldStage1());

                break;
            case ObjectRotState.moldy:

                break;
            case ObjectRotState.tranisitionToRot:
                
                foreach (Mold mold in moldsStage2)
                {
                    mold.GrowMold();
                }

                StartCoroutine(MoldStage2());

                break;
            case ObjectRotState.rotten:

                break;
        }
    }

    private void CheckRot(float damage)
    {
        if (currRotState == ObjectRotState.transitionToMold ||
            currRotState == ObjectRotState.tranisitionToRot ||
            currRotState == ObjectRotState.rotten) return; // if transitioning anim, don't take damage

        currHealth -= damage;

        // technically this could be cleaner but this way it's more readable
        if (currRotState == ObjectRotState.fresh && currHealth <= 0.7f * objectInfo.maxHealth)
        {
            SetState(ObjectRotState.transitionToMold);
        }
        else if (currRotState == ObjectRotState.moldy && currHealth <= 0.3f * objectInfo.maxHealth)
        {
            SetState(ObjectRotState.tranisitionToRot);
        }
    }

    private void Update()
    {
        CheckRot(damageDebuff);
    }

    private void AddDamageDebuff(float dmg)
    {
        damageDebuff += dmg;
    }

    #region Debug/Demo

    [ContextMenu("GrowMold")] // debug
    public void GrowAllMold()
    {
        foreach (Mold mold in moldsStage1)
        {
            mold.GrowMold();
        }

        foreach (Mold mold in moldsStage2)
        {
            mold.GrowMold();
        }
    }

    [ContextMenu("IncrementState")]
    public void IncrementState()
    {
        SetState(currRotState + 1);
    }

    # endregion

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
            AddDamageDebuff(bug.GetDamageDebuff());
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
    private IEnumerator MoldStage1()
    {
        yield return new WaitForSeconds(1);

        SetState(currRotState + 1);
    }

    private IEnumerator MoldStage2()
    {
        yield return new WaitForSeconds(2);

        rotAnimator.SetTrigger("Rot");
        //rotAnimator.Play("ToRot");

        yield return new WaitForSeconds(1);

        SetState(currRotState + 1);
    }

    #endregion
}
