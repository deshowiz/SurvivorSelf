using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private static EventManager eventManager;

    public delegate void ParamaterlessEvent();
    public static ParamaterlessEvent OnStartWave;
    public static ParamaterlessEvent OnEndWave;
    public static ParamaterlessEvent OnDeath;

    public delegate void FloatEvent(float floatP);
    public static FloatEvent OnPlayerHealthInitialization;
    public static FloatEvent OnPlayerHealthChange;

    public delegate void IntEvent(int intP);
    public static IntEvent OnTimerChange;

    public delegate void ItemEquippedEvent(Item newItem);
    public static ItemEquippedEvent OnItemEquipped;


    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindFirstObjectByType<EventManager>();

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    //eventManager.Init();

                    //  Sets this to not be destroyed when reloading scene
                    //DontDestroyOnLoad(eventManager);
                }
            }
            return eventManager;
        }
    }

    // public static void RegisterEvent<T>(string eventName, T param)
    // {

    // }

}

