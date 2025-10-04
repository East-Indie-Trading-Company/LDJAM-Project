using UnityEngine;

public abstract class DayAdvanceListener : MonoBehaviour
{
    protected virtual void Start()
    {
        if (FlagManager.Instance != null)
        {
            Debug.Log($"[DayAdvanceListener] Late subscription for {name}");
            FlagManager.Instance.OnDayAdvancedEvent += HandleDayAdvanced;
        }
    }
    protected virtual void OnEnable()
    {
        if (FlagManager.Instance != null)
            FlagManager.Instance.OnDayAdvancedEvent += HandleDayAdvanced;
    }

    protected virtual void OnDisable()
    {
        if (FlagManager.Instance != null)
            FlagManager.Instance.OnDayAdvancedEvent -= HandleDayAdvanced;
    }

    /// <summary>
    /// Method that concrete systems implement to react to day advance.
    /// </summary>
    protected abstract void HandleDayAdvanced(int newDay);
}
