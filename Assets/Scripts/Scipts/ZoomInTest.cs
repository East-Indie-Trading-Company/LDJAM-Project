using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ZoomInTest : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public float zoomDistance = 3f;
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    [Header("UI Panel")]
    public GameObject rightPanel;
    public Image npcImage; //  Add this reference in the Inspector

    public TownUI townUI;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isZoomed = false;
    private Transform target;

    

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalPosition = mainCamera.transform.position;
        originalRotation = mainCamera.transform.rotation;

        if (rightPanel != null)
            rightPanel.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("City"))
                {
                    target = hit.collider.transform;
                    isZoomed = true;

                    TownData townData = hit.collider.GetComponent<TownData>();

                    if (rightPanel != null)
                        rightPanel.SetActive(true);

                    if (townData != null && townUI != null)
                    {
                        
                        townUI.SetTownUI(townData.npcData); // 👈 this updates the NPC data [Arwen edit this to be updated with new implementation]
                    }
                }

            }
        }

        if (isZoomed && target != null)
        {
            Vector3 targetPosition = target.position - target.forward * zoomDistance + Vector3.up * 1f;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * moveSpeed);

            Quaternion lookRotation = Quaternion.LookRotation(target.position - mainCamera.transform.position);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalPosition, Time.deltaTime * moveSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, originalRotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void ResetZoom()
    {
        isZoomed = false;
        target = null;
        
        if (rightPanel != null)
            rightPanel.SetActive(false);
    }

    // UI button methods
    public void OnTalkPressed() { Debug.Log("Talk button pressed"); }
    public void OnRumorsPressed() { Debug.Log("Rumors button pressed"); }
    public void OnTradePressed() { Debug.Log("Trade button pressed"); }
    public void OnBackPressed() { ResetZoom(); }
}
