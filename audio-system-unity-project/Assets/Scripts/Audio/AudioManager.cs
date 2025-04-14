using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct VolumeValues
{
    [Range(0, 1)]
    public float masterVolume;
    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float ambienceVolume;
    [Range(0, 1)]
    public float sfxVolume;
}

public struct VolumeBuses
{
    public Bus masterBus;
    public Bus musicBus;
    public Bus ambienceBus;
    public Bus sfxBus;
}

public class AudioManager : MonoBehaviour
{
    private VolumeValues volumeSettings;
    private VolumeBuses volumeBuses;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    private EventInstance ambienceInstance;
    private EventInstance musicInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene!");
        }

        instance = this;
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        volumeBuses.masterBus = RuntimeManager.GetBus("bus:/");
        volumeBuses.musicBus = RuntimeManager.GetBus("bus:/Music");
        volumeBuses.sfxBus = RuntimeManager.GetBus("bus:/Sfx");
        volumeBuses.ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetAmbienceVolume(1f);
        SetSfxVolume(1f);
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.musicLevel1EventRef);
        SetMusicParameter(MusicArea.GRAY_AREA);

        InitializeAmbience(FMODEvents.instance.ambienceLevel1EventRef);
        SetAmbienceParameter("wind-intensity", 0);
    }

    public float GetMasterVolume()
    {
        return volumeSettings.masterVolume;
    }

    public float GetMusicVolume()
    {
        return volumeSettings.musicVolume;
    }   

    public float GetAmbienceVolume()
    {
        return volumeSettings.ambienceVolume;
    }

    public float GetSfxVolume()
    {
        return volumeSettings.sfxVolume;
    }

    public void SetMasterVolume(float value)
    {
        volumeSettings.masterVolume = value;
        volumeBuses.masterBus.setVolume(volumeSettings.masterVolume);
    }

    public void SetMusicVolume(float value)
    {
        volumeSettings.musicVolume = value;
        volumeBuses.musicBus.setVolume(volumeSettings.musicVolume);
    }

    public void SetAmbienceVolume(float value)
    {
        volumeSettings.ambienceVolume = value;
        volumeBuses.ambienceBus.setVolume(volumeSettings.ambienceVolume);
    }

    public void SetSfxVolume(float value)
    {
        volumeSettings.sfxVolume = value;
        volumeBuses.sfxBus.setVolume(volumeSettings.sfxVolume);
    }

    private void InitializeMusic(EventReference eventRef)
    {
        musicInstance = RuntimeManager.CreateInstance(eventRef);
        musicInstance.start();
    }

    public void SetMusicParameter(MusicArea musicArea)
    {
        musicInstance.setParameterByName("area", (float)musicArea);
    }

    private void InitializeAmbience(EventReference eventRef)
    {
        ambienceInstance = RuntimeManager.CreateInstance(eventRef);
        ambienceInstance.start();
    }

    public void SetAmbienceParameter(string parameterName, float value)
    {
        ambienceInstance.setParameterByName(parameterName, value);
    }

    public float GetAmbienceParameter(string parameterName)
    {
        float value;
        ambienceInstance.getParameterByName(parameterName, out value);

        return value;
    }

    public void PlayOneShot(EventReference eventRef, Vector3 position)
    {
        RuntimeManager.PlayOneShot(eventRef, position);
    }

    public EventInstance CreateInstance(EventReference eventRef)
    {
        var eventInstance = RuntimeManager.CreateInstance(eventRef);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter CreateEmitter(EventReference eventRef, Transform transform, EmitterGameEvent playEvent, EmitterGameEvent stopEvent, bool overrideAttenuation = false, float minDistance = 1f, float maxDistance = 6f)
    {
        var emitter = transform.gameObject.AddComponent<StudioEventEmitter>();
        Debug.Log($"Event ref {eventRef}");

        emitter.EventReference = eventRef;

        if (overrideAttenuation)
        {
            emitter.OverrideAttenuation = true;
            emitter.OverrideMinDistance = minDistance;
            emitter.OverrideMaxDistance = maxDistance;
        }

        emitter.EventPlayTrigger = playEvent;
        emitter.EventStopTrigger = stopEvent;

        eventEmitters.Add(emitter);

        return emitter;
    }

    private void CleanUp()
    {
        foreach (var item in eventInstances)
        {
            item.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            item.release();
        }

        eventInstances.Clear();

        foreach (var item in eventEmitters)
        {
            item.Stop();
        }

        eventEmitters.Clear();

        ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();

        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

    private void OnDestroy()
    {
        CleanUp();
    }

}
