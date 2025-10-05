using UnityEngine;

namespace Trading
{
    [CreateAssetMenu(menuName = "Reputation", fileName = "Reputation")]
    public class WorldReputationSO : ScriptableObject
    {
        [Header("Designer Settings")]
        [Tooltip("Minimum and maximum possible reputation values. Example: -100 = hated, +100 = loved.")]
        public float minValue = -100f;
        public float maxValue = 100f;

        [Header("Current Reputation")]
        [Tooltip("Current world reputation (clamped between minValue and maxValue).")]
        public float currentValue = 0f;

        /// <summary>
        /// Returns a normalized reputation for systems like Economy or PriceCalculator.
        /// </summary>
        public float NormalizedValue
        {
            get
            {
                if (Mathf.Approximately(maxValue, minValue))
                    return 0f;

                float clamped = Mathf.Clamp(currentValue, minValue, maxValue);

                // Map to -1..+1
                return Mathf.InverseLerp(minValue, maxValue, clamped) * 2f - 1f;
            }
        }

        [ContextMenu("Reset Reputation")]
        public void ResetReputation()
        {
            currentValue = 0f;
        }

        [ContextMenu("Max Reputation")]
        public void MaxReputation()
        {
            currentValue = maxValue;
        }

        [ContextMenu("Min Reputation")]
        public void MinReputation()
        {
            currentValue = minValue;
        }
    }
}
