using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }

    private Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private Dictionary<string, int> counters = new Dictionary<string, int>();

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

    public void OnDayAdvanced(int newDay)
    {
        OnDayAdvancedEvent?.Invoke(newDay);
    }
}