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