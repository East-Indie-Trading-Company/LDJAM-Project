using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private Dictionary<string, int> counters = new Dictionary<string, int>();

    // Evento p�blico que outros sistemas v�o escutar
    public event Action<int> OnDayAdvancedEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAllFlags();
    }

    public void SetFlag(string flag, bool value)
    {
        flags[flag] = value;
    }

    public bool GetFlag(string flag)
    {
        return flags.ContainsKey(flag) && flags[flag];
    }

    public void AddCounter(string key, int value)
    {
        if (!counters.ContainsKey(key)) counters[key] = 0;
        counters[key] += value;
    }

    public int GetCounter(string key)
    {
        return counters.ContainsKey(key) ? counters[key] : 0;
    }

    // Novo nome, mais claro: dispara os callbacks de "dia avan�ado"
    public void TriggerDayAdvanced(int newDay)
    {
        Debug.Log($"[FlagManager] Triggering Day Advanced {newDay}");
        OnDayAdvancedEvent?.Invoke(newDay);
    }

    public void TriggerMilestone(string actFlag, string milestoneName, int funding)
    {
        Debug.Log($"TRIGGER ACT {actFlag}");

        SetFlag(actFlag, true);
        switch (actFlag)
        {
            case "Act1":
                SetFlag("Act3", false);
                SetFlag("Act2", false);
                break;
            case "Act2":
                SetFlag("Act1", false);
                SetFlag("Act3", false);
                break;
            case "Act3":
                SetFlag("Act1", false);
                SetFlag("Act2", false);
                break;

        }
        Debug.Log($"[FlagManager] {milestoneName} reached! Funding: ${funding:N0}");
    }


    public void InitializeAllFlags()
    {
        string DragonDeath = "DragonDeath";
        string EndGameDragon = "EndGameDragon";
        string EndGameKingdom = "EndGameKingdom";
        string Act1 = "Act1";
        string Act2 = "Act2";
        string Act3 = "Act3";
        string LowRep = "LowRep";
        string HighRep = "HighRep";
        string Dragon1 = "Dragon1";
        string Dragon2 = "Dragon2";
        string Dragon3 = "Dragon3";


        SetFlag(DragonDeath, false);
        SetFlag(EndGameDragon, false);
        SetFlag(EndGameKingdom, false);
        SetFlag(Act1, true);
        SetFlag(Act2, false);
        SetFlag(Act3, false);
        SetFlag(LowRep, false);
        SetFlag(HighRep, false);
        SetFlag(Dragon1, false);
        SetFlag(Dragon2, false);
        SetFlag(Dragon3, false);
    }
}