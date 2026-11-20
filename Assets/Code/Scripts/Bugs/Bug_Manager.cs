using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bug_Manager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Bug_Info bugInfo;
    private SpriteRenderer spriteRenderer;
    private Steering_Controller steeringController;
    private bool isDead;
    private bool isLanded;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        steeringController = GetComponent<Steering_Controller>();
        animator = GetComponent<Animator>();
    }

    public Bug_Info GetBugInfo()
    {
        return bugInfo;
    }

	public void OnPointerClick(PointerEventData data)
    {
        // death
        if (!isDead)
        {
            StartCoroutine(BugDeathSequence());
        }
    }

    // makes bug have a "recoil" pause before death animation
    IEnumerator BugDeathSequence()
    {
        isDead = true;
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        GetComponent<CircleCollider2D>().enabled = false;
        animator.SetTrigger("Die");

        // freeze position (none of these doing anyuthing)
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.Sleep();

        yield return new WaitForSeconds(.32f);
        
        // let fall
        rb.WakeUp();
        rb.mass = 2f;
    }

    public void CheckFlipSprite(Vector3 velocity)
    {
        if (spriteRenderer.flipX && velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (!spriteRenderer.flipX && velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public void Land()
    {
        if (!isLanded)
        {
            animator.SetTrigger("Land");
            // update sprite
            spriteRenderer.sprite = bugInfo.landedSprite;

            // turn off steering behavior
            steeringController.PauseSteeringBehaviors();

            // turn on small movement behaviors
            steeringController.StartLandedBehaviors();
        }
    }

    public void Fly()
    {
        isLanded = false;
        animator.SetTrigger("Land");
        spriteRenderer.sprite = bugInfo.flyingSprite;
    }
}
