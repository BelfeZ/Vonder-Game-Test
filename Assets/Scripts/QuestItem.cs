using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public NPCManager targetNPC;
    public GameObject interactPrompt;

    private bool playerInRange = false;

    private void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        if (targetNPC != null)
        {
            targetNPC.hasQuestItem = true;
        }

        QuestArrowManager arrow = FindFirstObjectByType<QuestArrowManager>();

        if (arrow != null)
            arrow.ClearTarget();

        BannerManager.Instance.ShowBanner("Got Item!");
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }
}
