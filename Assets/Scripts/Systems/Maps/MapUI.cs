using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MapUI : MonoBehaviour
{
    [Header("Town Buttons")]
    [SerializeField] Button brightspireButton;
    [SerializeField] Button trestelButton;
    [SerializeField] Button dursimButton;
    [SerializeField] Button styxButton;
    [SerializeField] Button lochButton;
    [SerializeField] Button scorchedButton;
    [SerializeField] Button taravalButton;

    [Header("NPC References")]
    [SerializeField] NPCData brightspireData;
    [SerializeField] NPCData trestelData;
    [SerializeField] NPCData dursimData;
    [SerializeField] NPCData styxData;
    [SerializeField] NPCData lochData;
    [SerializeField] NPCData scorchedData;
    [SerializeField] NPCData taravalData;

    [Header("UI Panels")]
    [SerializeField] GameObject mapButtonUI;
    [SerializeField] GameObject mapBackButtonUI;
    [SerializeField] TownUI townUI;

    [Header("Camera Settings")]
    [SerializeField] Transform[] townPositions;  // each town transform (0=Brightspire, etc.)
    [SerializeField] Transform mapViewPosition;
    [SerializeField] float zoomDuration = 1.0f;
    [SerializeField] float stopDistance = 3f;

    private bool isMoving = false;
    private Button backButton;

    void Start()
    {
        // Assign buttons
        backButton = mapBackButtonUI.GetComponent<Button>();
        mapButtonUI.SetActive(true);
        mapBackButtonUI.SetActive(false);

        backButton.onClick.AddListener(OnBackClick);

        brightspireButton.onClick.AddListener(() => OnTownClick(brightspireData, 0));
        trestelButton.onClick.AddListener(() => OnTownClick(trestelData, 1));
        dursimButton.onClick.AddListener(() => OnTownClick(dursimData, 2));
        styxButton.onClick.AddListener(() => OnTownClick(styxData, 3));
        lochButton.onClick.AddListener(() => OnTownClick(lochData, 4));
        scorchedButton.onClick.AddListener(() => OnTownClick(scorchedData, 5));
        taravalButton.onClick.AddListener(() => OnTownClick(taravalData, 6));
    }

    void OnTownClick(NPCData npcData, int index)
    {
        if (isMoving) return;
        if (npcData == null)
        {
            Debug.LogWarning($"[MapUI] NPCData missing for town index {index}");
            return;
        }

        // --- Game logic: advance time ---
        GameManager.Instance?.AdvanceDay();

        // --- UI logic ---
        mapButtonUI.SetActive(false);
        mapBackButtonUI.SetActive(true);
        townUI.SetTownUI(npcData);

        // --- Camera zoom ---
        if (index >= 0 && index < townPositions.Length)
            StartCoroutine(ZoomCameraCoroutine(townPositions[index]));
    }

    void OnBackClick()
    {
        if (isMoving) return;

        mapButtonUI.SetActive(true);
        mapBackButtonUI.SetActive(false);
        townUI.RemoveTownUI();

        // Move camera back to map view
        StartCoroutine(ZoomCameraCoroutine(mapViewPosition));
        if (FlagManager.Instance.GetFlag("Dragon1"))
        {
            DialogueManager.Instance.StartDialogue(GameManager.Instance.act1Convo);
            FlagManager.Instance.SetFlag("Dragon1", false);
        }
        else if (FlagManager.Instance.GetFlag("Dragon2"))
        {
            DialogueManager.Instance.StartDialogue(GameManager.Instance.act2Convo);
            FlagManager.Instance.SetFlag("Dragon2", false);
        }
        else if (FlagManager.Instance.GetFlag("Dragon3"))
        {
            DialogueManager.Instance.StartDialogue(GameManager.Instance.act3Convo);
            FlagManager.Instance.SetFlag("Dragon3", false);
        }
    }

    private IEnumerator ZoomCameraCoroutine(Transform target)
    {
        isMoving = true;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[MapUI] No MainCamera found!");
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

        //Debug.Log($"[MapUI] Zoom finished → {target.name}");
    }
}
