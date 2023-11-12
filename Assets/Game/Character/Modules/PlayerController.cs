using System;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        Right,
        Left
    }

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private FacingDirection facingDirection = FacingDirection.Right;
    private bool levelStarted = false;
    private bool isInDamagableArea = false;
    private Damagable.DamagableData damagableInArea;

    public int numberOfPoints = 0;
    public int health = 5000;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private Transform groundCheckTransform;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private Transform startMarker;

    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Text pointsValueText;

    [SerializeField]
    private Text healthValueText;


    private bool IsGrounded => Physics2D.OverlapCircle(groundCheckTransform.position, 0.2f, groundLayerMask);

    void Start()
    {
        transform.position = startMarker.position + new Vector3(2, 0);
        Debug.Log("Level Started");
        levelStarted = true;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        transform.rotation = new Quaternion(0, 0, 0, 1);

        JumpIfNeeded();
        SpeedUpJumpIfNeeded();
        FlipPlayerFacing();

        UpdateAnimator();
        CheckIfDamageShouldBeGiven();
        UpdateValueLabels();
    }

    private void UpdateValueLabels()
    {
        pointsValueText.text = numberOfPoints.ToString();
        healthValueText.text = health.ToString();
    }

    private void CheckIfDamageShouldBeGiven()
    {
       if(isInDamagableArea && damagableInArea != null)
       {
            health -= damagableInArea.HealthDamage;  
            Debug.Log($"Damaged with damagable with id {damagableInArea.Id}. Number of points now {numberOfPoints}. Health status {health}");
       }
    }

    private void UpdateAnimator()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        animator.SetFloat("velocityX", Mathf.Abs(horizontalInput));
        animator.SetFloat("velocityY", Mathf.Abs(verticalInput));
    }

    private void JumpIfNeeded()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpingPower);
        }
    }

    private void SpeedUpJumpIfNeeded()
    {
        if (Input.GetButtonDown("Jump") && playerRigidbody.velocity.y > 0f)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
        }
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = new Vector2(horizontal * speed, playerRigidbody.velocity.y);
    }

    private void FlipPlayerFacing()
    {
        var switchedToLeft = facingDirection == FacingDirection.Right && horizontal < 0f;
        var switchedToRight = facingDirection == FacingDirection.Left && horizontal > 0f;
        if (switchedToLeft || switchedToRight)
        {
            facingDirection = switchedToLeft ? FacingDirection.Left : FacingDirection.Right;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EndMarker") && levelStarted)
        {
            Debug.Log("Level Completed");
        }

        if (other.CompareTag("CollectibleMarker"))
        {
            var collectiblePrefab = other.GetComponent<BasicCollectible>();
            var collectibleData = collectiblePrefab.Data;

            numberOfPoints += collectibleData.PointsToGain;

            dataManager.DestroyCollectibleWithId(collectibleData.Id);

            Debug.Log($"Collected collectible with id {collectibleData.Id}. Number of points now {numberOfPoints}. Health status {health}");
        }

        if (other.CompareTag("DamagableMarker"))
        {
            isInDamagableArea = true;
            var damagablePrefab = other.GetComponent<Damagable>();
            var damagableData = damagablePrefab.Data;
            damagableInArea = damagableData;

            health -= damagableData.HealthDamage;

            Debug.Log($"Damaged with damagable with id {damagableData.Id}. Number of points now {numberOfPoints}. Health status {health}");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
         if (other.CompareTag("DamagableMarker"))
        {
            isInDamagableArea = false;
            damagableInArea = null;
        }
    }
}
