using UnityEngine;

public class ReputationManager: MonoBehaviour
{
    public static ReputationManager Instance { get; private set; }

    [SerializeField] private float currentReputation = 0f;
    [SerializeField] private Vector2 reputationBounds = new Vector2(-1f,1f);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        PermaUIManager.Instance?.SetReputationUI(currentReputation);
    }

    public float GetReputation()
    {
        return currentReputation;
    }

    public void SetReputation( float newReputation )
    {
        newReputation = Mathf.Clamp(newReputation, reputationBounds.x, reputationBounds.y);
        if (newReputation < 0)
        {
            FlagManager.Instance.SetFlag("HighRep", false);
            FlagManager.Instance.SetFlag("LowRep", true);
        }
        else if (newReputation > 0)
        {
            FlagManager.Instance.SetFlag("HighRep", true);
            FlagManager.Instance.SetFlag("LowRep", false);
        }
        else
        {
            FlagManager.Instance.SetFlag("HighRep", false);
            FlagManager.Instance.SetFlag("LowRep", false);
        }
        currentReputation = newReputation;
    }

    public void AddReputation( float reputationToAdd )
    {
        SetReputation(currentReputation += reputationToAdd);
    }

    public void SubtractReputation(float reputationToSubtract)
    {
        SetReputation(currentReputation -= reputationToSubtract);
    }
}
