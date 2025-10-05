using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapManager : MonoBehaviour
{
    [SerializeField] Transform target; // Either one of the towns or the entire map
    //[SerializeField] Button town1Button;
    //public Vector3 targetPosition = new Vector3(0, 5, -10);
    public float zoomDuration = 1.0f;
    public float zoomSpeed = 5f;
    public float stopDistance = 3f;

    private bool isMoving = false;

    // Makes the camera zoom out to show the full map
    public void TravelBackToMap()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    
    // Each of these functions makes the camera zoom in to each corresponding town on the map
    public void TravelToTown1()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    public void TravelToTown2()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    public void TravelToTown3()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }

    }
    public void TravelToTown4()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    public void TravelToTown5()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    public void TravelToTown6()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }
    public void TravelToTown7()
    {
        if (!isMoving)
        {
            StartCoroutine(ZoomCameraCoroutine());
        }
    }

    private IEnumerator ZoomCameraCoroutine()
    {
        isMoving = true;
        Vector3 startPosition = Camera.main.transform.position;
        Vector3 targetPosition = target.position - (target.forward * stopDistance);
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / zoomDuration);
            //transform.rotation = Quaternion.Slerp(startRotation, targetPosition.rotation, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the camera reaches the exact target position and rotation
        //Camera.main.transform.position = target.position;
        Camera.main.transform.position = targetPosition;
        //transform.rotation = targetPosition.rotation;
        isMoving = false;
    }
}
