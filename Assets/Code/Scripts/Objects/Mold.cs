using System.Collections;
using UnityEngine;

public class Mold : MonoBehaviour
{
    [SerializeField] private Animator moldAnimator;
    [SerializeField] private bool debugForceEnableMold;

    private void Start()
    {
        moldAnimator.enabled = true;
    }

    public void GrowMold()
    {
        StartCoroutine(WaitRandomThenGrow());
    }

    IEnumerator WaitRandomThenGrow()
    {
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        moldAnimator.SetTrigger("Sprout");
    }
}
