using UnityEngine;

public class DayTester : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 200), GUI.skin.box);

        GUILayout.Label($"Current Day: {GameManager.Instance.CurrentDay}");

        if (GUILayout.Button("Advance 1 Day"))
        {
            GameManager.Instance.AdvanceDay();
        }

        if (GUILayout.Button("Advance 5 Days"))
        {
            GameManager.Instance.AdvanceDays(5);
        }

        if (GUILayout.Button("Advance 10 Days"))
        {
            GameManager.Instance.AdvanceDays(10);
        }

        GUILayout.EndArea();
    }
}
