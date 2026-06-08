using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("Interaction Configuration")]
    public float interactionRadius = 2f;

    private Rigidbody2D rb;
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private Animator animator;
    private bool isTalking = false;
    private float interactCooldown = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controls = new InputSystem_Actions();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        if (interactCooldown > 0f)
            interactCooldown -= Time.deltaTime;

        if (Keyboard.current.eKey.wasPressedThisFrame && interactCooldown <= 0f)
        {
            if (QuestManager.Instance == null || !QuestManager.Instance.dialoguePanel.activeSelf)
            {
                Interact();
            }
        }

        if (isTalking)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isRun", false);
            animator.SetBool("idle", true);
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        bool movingHorizontally = Mathf.Abs(moveInput.x) > 0.1f;
        if (!isGrounded)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isRun", false);
            animator.SetBool("idle", false);
        }
        else if (movingHorizontally)
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isRun", true);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isRun", false);
            animator.SetBool("idle", true);
        }
    }

    private void FixedUpdate()
    {
        if (isTalking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void Jump()
    {
        if (isGrounded && !isTalking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayJump();
        }
    }

    private void Interact()
    {

        if (QuestManager.Instance != null && QuestManager.Instance.dialoguePanel.activeSelf)
        {
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

        foreach (Collider2D col in hitColliders)
        {
            NPCManager npc = col.GetComponentInParent<NPCManager>();

            if (npc != null && npc.IsPlayerInRange() && npc.CanInteract())
            {
                isTalking = true;
                npc.InteractWithNPC(this.transform);
                break;
            }
        }
    }

    public void EndTalking()
    {
        isTalking = false;
        interactCooldown = 0.5f;
    }

    public void PlayFootstepSound()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWalk();
    }
}