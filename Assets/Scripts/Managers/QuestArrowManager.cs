using UnityEngine;

public class QuestArrowManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Camera mainCamera;
    public float screenEdgePadding = 60f;

    private Transform target;
    private RectTransform arrowRect;
    private GameObject arrowInstance;

    private void Awake()
    {
        arrowInstance = Instantiate(arrowPrefab, transform);
        arrowRect = arrowInstance.GetComponent<RectTransform>();
        arrowInstance.SetActive(false);
    }

    private void Update()
    {
        if (target == null)
        {
            arrowInstance.SetActive(false);
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
            arrowInstance.SetActive(false);
            return;
        }

        arrowInstance.SetActive(true);

        if (screenPos.z < 0)
            screenPos *= -1;

        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 dir = ((Vector2)screenPos - center).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrowRect.rotation = Quaternion.Euler(0, 0, angle);

        Vector2 edgePos = center + dir * (Mathf.Min(Screen.width, Screen.height) * 0.4f);

        edgePos.x = Mathf.Clamp(edgePos.x, screenEdgePadding, Screen.width - screenEdgePadding);
        edgePos.y = Mathf.Clamp(edgePos.y, screenEdgePadding, Screen.height - screenEdgePadding);

        arrowRect.position = edgePos;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;

        if (arrowInstance != null)
            arrowInstance.SetActive(false);
    }
}
