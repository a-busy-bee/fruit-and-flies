using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bug_Manager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Bug_Info bugInfo;
    private SpriteRenderer spriteRenderer;
    private Steering_Controller steeringController;
    private Landed_Controller landedController;
    private bool isDead;
    private bool isLanded;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        steeringController = GetComponent<Steering_Controller>();
        landedController = GetComponent<Landed_Controller>();
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
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            GetComponent<CircleCollider2D>().enabled = false;

            rb.mass = 2f;
            isDead = true; 

            LevelManager.instance.KillFly();

            StartCoroutine(WaitThenDestroy()); 
        }
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
        if (!isLanded && landedController != null)
        {
            // update sprite
            spriteRenderer.sprite = bugInfo.landedSprite;

            // turn off steering behavior
            steeringController.PauseSteeringBehaviors();

            // turn on small movement behaviors
            landedController.StartLandedBehaviors();
        }
    }

    public void Fly()
    {
        isLanded = false;
        steeringController.RestoreSteeringBehaviors();

        spriteRenderer.sprite = bugInfo.flyingSprite;
    }

    private IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
