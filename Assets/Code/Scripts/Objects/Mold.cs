using UnityEngine;

public class Mold : MonoBehaviour
{
    [SerializeField] private Animator moldAnimator;

    private void Start()
    {
        moldAnimator.enabled = false;
    }

    public void GrowMold()
    {
        moldAnimator.enabled = true;
        moldAnimator.Play("Grow");
    }
}
