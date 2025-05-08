using System;
using System.Collections.Generic;

public class GameState
{
    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }
        }
    }
    #endregion

    #region bool isFpv
    private static bool _isFpv = true;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }
        }
    }
    #endregion

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();
    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
    }
    public static void RemoveListener(Action<string> listener)
    {
        listeners.Remove(listener);
    }
    private static void Notify(string fieldName)
    {
        foreach (var listener in listeners)
        {
            listener.Invoke(fieldName);
        }
    }
    #endregion


    #region bool isKey1Collected
    private static bool isKey1Collected = false;
    public static bool IsKey1Collected
    {
        get => isKey1Collected;
        set
        {
            if (isKey1Collected != value)
            {
                isKey1Collected = value;
                Notify(nameof(IsKey1Collected));
            }
        }
    }
    #endregion
}