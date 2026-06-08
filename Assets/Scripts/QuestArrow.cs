using UnityEngine;

public class QuestArrow : MonoBehaviour
{
    public Transform target;
    public Camera mainCamera;

    [SerializeField] private RectTransform arrowRect;
    [SerializeField] private float screenEdgePadding = 50f;

    private bool isVisible;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (arrowRect == null)
            arrowRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (target == null)
        {
            arrowRect.gameObject.SetActive(false);
            return;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        bool onScreen =
            screenPos.z > 0 &&
            screenPos.x > 0 &&
            screenPos.x < Screen.width &&
            screenPos.y > 0 &&
            screenPos.y < Screen.height;

        if (onScreen)
        {
            if (isVisible)
            {
                arrowRect.gameObject.SetActive(false);
                isVisible = false;
            }
            return;
        }

        if (!isVisible)
        {
            arrowRect.gameObject.SetActive(true);
            isVisible = true;
        }

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 dir = ((Vector2)screenPos - center).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowRect.rotation = Quaternion.Euler(0, 0, angle);

        Vector2 edgePos = center + dir *
            (Mathf.Min(Screen.width, Screen.height) * 0.4f);

        edgePos.x = Mathf.Clamp(edgePos.x,
            screenEdgePadding,
            Screen.width - screenEdgePadding);

        edgePos.y = Mathf.Clamp(edgePos.y,
            screenEdgePadding,
            Screen.height - screenEdgePadding);

        arrowRect.position = edgePos;
    }
}
