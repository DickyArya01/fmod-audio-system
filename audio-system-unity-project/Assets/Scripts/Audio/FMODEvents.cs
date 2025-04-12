using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambienceLevel1EventRef { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference musicLevel1EventRef { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootstepsEventRef { get; private set; }
    
    [field: Header("Coin SFX")]
    [field: SerializeField] public EventReference coinIdleEventRef { get; private set; }
    [field: SerializeField] public EventReference coinCollectedEventRef { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one FMODEvents in the scene!");
        }

        instance = this;
    }
}
