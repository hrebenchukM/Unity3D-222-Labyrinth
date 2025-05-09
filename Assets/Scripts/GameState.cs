using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class GameState : MonoBehaviour
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



    #region bool isKey1InTime
    private static bool isKey1InTime = false;
    public static bool IsKey1InTime
    {
        get => isKey1InTime;
        set
        {
            if (isKey1Collected != value)
            {
                isKey1Collected = value;
                Notify(nameof(IsKey1InTime));
            }
        }
    }
    #endregion

    #region bool isKey2InTime
    private static bool isKey2InTime = false;
    public static bool IsKey2InTime
    {
        get => isKey2InTime;
        set
        {
            if (isKey2Collected != value)
            {
                isKey2Collected = value;
                Notify(nameof(IsKey2InTime));
            }
        }
    }
    #endregion



    #region bool isKey2Collected
    private static bool isKey2Collected = false;
    public static bool IsKey2Collected
    {
        get => isKey2Collected;
        set
        {
            if (isKey2Collected != value)
            {
                isKey2Collected = value;
                Notify(nameof(IsKey2Collected));
            }
        }
    }
    #endregion






    #region bool isKey3InTime
    private static bool isKey3InTime = false;
    public static bool IsKey3InTime
    {
        get => isKey3InTime;
        set
        {
            if (isKey3Collected != value)
            {
                isKey3Collected = value;
                Notify(nameof(IsKey3InTime));
            }
        }
    }
    #endregion



    #region bool isKey3Collected
    private static bool isKey3Collected = false;
    public static bool IsKey3Collected
    {
        get => isKey3Collected;
        set
        {
            if (isKey3Collected != value)
            {
                isKey3Collected = value;
                Notify(nameof(IsKey3Collected));
            }
        }
    }
    #endregion



    public static object GetProperty(string propertyName)
    {
        var property = typeof(GameState).GetProperty(
            propertyName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static
            );
        if(property == null )
        {
            UnityEngine.Debug.LogError($"Property not found:'{propertyName}'");
            return null;
        }
        return property.GetValue(null);
    }
    public static void SetProperty(string propertyName,object value)
    {
        var property = typeof(GameState).GetProperty(
            propertyName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Static
            );
        if (property == null)
        {
            UnityEngine.Debug.LogError($"Property not found:'{propertyName}' setting value '{value}'");
        }
        else 
        {
            property.SetValue(null, value); 
        }
    }
}