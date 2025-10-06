using UnityEngine;
using System.Collections;

public class MapManager1 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] towns;   // Town 1..7 in order
    [SerializeField] private Transform mapView;   // overview position
    [SerializeField] private GameManager gameManager;

    [Header("Camera Settings")]
    [SerializeField] private float zoomDuration = 1.0f;
    [SerializeField] private float stopDistance = 3f;

    private bool isMoving = false;

    // Buttons call these
    public void TravelToTown1() => TravelToTown(0);
    public void TravelToTown2() => TravelToTown(1);
    public void TravelToTown3() => TravelToTown(2);
    public void TravelToTown4() => TravelToTown(3);
    public void TravelToTown5() => TravelToTown(4);
    public void TravelToTown6() => TravelToTown(5);
    public void TravelToTown7() => TravelToTown(6);

    public void TravelBackToMap()
    {
        if (isMoving) return;
        StartCoroutine(ZoomCameraCoroutine(mapView));
        gameManager.ReturnToMap();
    }

    private void TravelToTown(int index)
    {
        if (isMoving || index < 0 || index >= towns.Length)
            return;

        Transform targetTown = towns[index];
        StartCoroutine(ZoomCameraCoroutine(targetTown));

        // tell GameManager to handle day logic
        TownData data = targetTown.GetComponent<TownData>();
        if (data != null)
            gameManager.TravelToTown(data);
    }

    private IEnumerator ZoomCameraCoroutine(Transform target)
    {
        isMoving = true;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[MapManager] No MainCamera found! Add tag 'MainCamera' to your camera.");
            yield break;
        }

        Vector3 startPosition = cam.transform.position;
        Vector3 targetPosition = target.position + new Vector3(0, 3f, -stopDistance);

        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            cam.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = targetPosition;
        isMoving = false;

        Debug.Log($"[MapManager] Zoom finished → {target.name}");
    }
}

