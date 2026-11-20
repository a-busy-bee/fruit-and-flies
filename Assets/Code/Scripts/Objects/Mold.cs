using System.Collections;
using UnityEngine;

public class Mold : MonoBehaviour
{
    [SerializeField] private Animator moldAnimator;
    [SerializeField] private bool debugForceEnableMold;

    private void Start()
    {
        if (moldAnimator == null)
        {
            Debug.Log("no mold animator");
            return;
        }
        moldAnimator.enabled = true;
    }

    public void GrowMold()
    {
        if (moldAnimator == null)
        {
            Debug.Log("no mold animator");
            return;
        }
        StartCoroutine(WaitRandomThenGrow());
    }

    IEnumerator WaitRandomThenGrow()
    {
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        moldAnimator.SetTrigger("Sprout");
    }
}
