using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeMenu : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstSelected;

    [Header("Player")]
    [SerializeField] private CharacterController2D characterController;

    [Header("Sliders")]
    [SerializeField] private UnityEngine.UI.Slider masterSlider;
    [SerializeField] private UnityEngine.UI.Slider musicSlider;
    [SerializeField] private UnityEngine.UI.Slider sfxSlider;
    [SerializeField] private UnityEngine.UI.Slider ambienceSlider;

    private void OnDestroy()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        ambienceSlider.onValueChanged.RemoveAllListeners();
    }

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMasterVolume(masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetMusicVolume(musicSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetSfxVolume(sfxSlider.value); });
        ambienceSlider.onValueChanged.AddListener(delegate { AudioManager.instance.SetAmbienceVolume(ambienceSlider.value); });

        masterSlider.value = AudioManager.instance.GetMasterVolume();
        musicSlider.value = AudioManager.instance.GetMusicVolume();
        sfxSlider.value = AudioManager.instance.GetSfxVolume();
        ambienceSlider.value = AudioManager.instance.GetAmbienceVolume();

        menu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.instance.GetPausePressed())
        {
            ToggleVolumeMenu();
        }
    }

    private void ToggleVolumeMenu()
    {
        // toggle menu OFF
        if (menu.gameObject.activeInHierarchy)
        {
            menu.gameObject.SetActive(false);
            characterController.EnableMovement();
        }
        // toggle menu ON
        else
        {
            menu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelected);
            characterController.DisableMovement();
        }
    }
}
