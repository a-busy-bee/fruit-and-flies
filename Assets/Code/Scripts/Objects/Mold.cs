using UnityEngine;

public class Mold : MonoBehaviour
{
    [SerializeField] private Animator moldAnimator;
    [SerializeField] private bool debugForceEnableMold;

    private void Start()
    {
        if (debugForceEnableMold)
        {
            moldAnimator.enabled = true;
        }
        else
        {
            moldAnimator.enabled = false;
        }
    }

    public void GrowMold()
    {
        moldAnimator.enabled = true;
        moldAnimator.Play("Grow");
    }
}
