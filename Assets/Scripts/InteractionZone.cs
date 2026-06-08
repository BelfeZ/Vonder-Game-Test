using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    private NPCManager npc;

    private void Awake()
    {
        npc = GetComponentInParent<NPCManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        npc.HandleTriggerEnter(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        npc.HandleTriggerExit(collision);
    }
}