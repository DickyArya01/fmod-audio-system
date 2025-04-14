using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Parameter change")]
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    float prevParameterValue;

    bool alreadyGetPassed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!alreadyGetPassed)
            {
                prevParameterValue = AudioManager.instance.GetAmbienceParameter(parameterName);
                AudioManager.instance.SetAmbienceParameter(parameterName, parameterValue);
                alreadyGetPassed = true;
            }
            else
            {
                AudioManager.instance.SetAmbienceParameter(parameterName, prevParameterValue);
                alreadyGetPassed = false;
            }
        }

    }
}
