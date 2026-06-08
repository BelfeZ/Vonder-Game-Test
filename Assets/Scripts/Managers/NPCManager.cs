using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactPrompt;

    [Header("Pinpoints")]
    public Transform[] pinpoints;
    public float walkSpeed = 2f;
    public float minWaitTime = 2f;
    public float maxWaitTime = 4f;

    [Header("Quest")]
    public ScriptableData scenarioData_Start;
    public ScriptableData scenarioData_End;
    public GameObject questItem;
    public QuestArrowManager questArrow;

    [Header("Quest Tracking State (For test)")]
    public bool hasStartedQuest = false;
    public bool hasQuestItem = false;

    private Rigidbody2D rb;
    private Animator animator;
    private int currentPinpointIndex = 0;
    private bool isMoving = false;
    private bool isTalking = false;
    private bool playerInRange = false;
    private bool isFacingRight = true;
    private bool itemSpawned = false;
    private bool isQuestCompleted = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (questItem != null)
            questItem.SetActive(false);

        if (pinpoints != null && pinpoints.Length > 0)
        {
            StartCoroutine(PatrolRoutine());
        }
    }

    private void Update()
    {
        if (isQuestCompleted)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isTalking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimationStates(idle: false, walk: false, talk: true);
            return;
        }

        if (isMoving)
        {
            Vector2 currentPos = transform.position;
            Vector2 targetPos = pinpoints[currentPinpointIndex].position;
            float distanceToTarget = Mathf.Abs(targetPos.x - currentPos.x);

            if (distanceToTarget > 0.1f)
            {
                float directionX = Mathf.Sign(targetPos.x - currentPos.x);
                rb.linearVelocity = new Vector2(directionX * walkSpeed, rb.linearVelocity.y);
                SetAnimationStates(idle: false, walk: true, talk: false);

                if (directionX < 0 && !isFacingRight) Flip();
                else if (directionX > 0 && isFacingRight) Flip();
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                isMoving = false;
            }
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (!isQuestCompleted)
        {
            if (!isTalking && !isMoving)
            {
                SetAnimationStates(idle: true, walk: false, talk: false);
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                if (!isTalking)
                {
                    currentPinpointIndex = (currentPinpointIndex + 1) % pinpoints.Length;
                    isMoving = true;
                }
            }
            yield return null;
        }
    }

    public void InteractWithNPC(Transform playerTransform)
    {
        if (!CanInteract()) return;

        isTalking = true;
        isMoving = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (playerTransform.position.x > transform.position.x && isFacingRight) Flip();
        else if (playerTransform.position.x < transform.position.x && !isFacingRight) Flip();

        SetAnimationStates(idle: false, walk: false, talk: true);

        if (!hasStartedQuest)
        {
            hasStartedQuest = true;
            //BannerManager.Instance.ShowBanner("Quest Started!");
            QuestManager.Instance.StartScenario(scenarioData_Start);
        }
        else if (hasQuestItem)
        {
            isQuestCompleted = true;

            //BannerManager.Instance.ShowBanner("Quest Completed!");
            QuestManager.Instance.StartScenario(scenarioData_End);
        }
    }

    public bool CanInteract()
    {
        return !isTalking && (!hasStartedQuest || hasQuestItem);
    }

    public void EndDialogue()
    {
        if (!isTalking) return;

        isTalking = false;

        if (!isQuestCompleted)
        {
            SetAnimationStates(idle: true, walk: false, talk: false);
        }

        if (hasStartedQuest && !hasQuestItem && !itemSpawned)
        {
            itemSpawned = true;

            if (questItem != null)
            {
                questItem.SetActive(true);

                if (questArrow != null)
                    questArrow.SetTarget(questItem.transform);
            }
        }

        if (interactPrompt != null)
            interactPrompt.SetActive(playerInRange && CanInteract());
    }

    private void SetAnimationStates(bool idle, bool walk, bool talk)
    {
        if (animator == null) return;
        animator.SetBool("idle", idle);
        animator.SetBool("isWalk", walk);
        animator.SetBool("isTalk", talk);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void HandleTriggerEnter(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(CanInteract());
    }

    public void HandleTriggerExit(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        EndDialogue();
    }

    public void PrepareForVanish()
    {
        isQuestCompleted = true;
        isTalking = false;
        isMoving = false;

        rb.linearVelocity = Vector2.zero;

        SetAnimationStates(idle: true, walk: false, talk: false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public bool IsPlayerInRange() => playerInRange;
}
