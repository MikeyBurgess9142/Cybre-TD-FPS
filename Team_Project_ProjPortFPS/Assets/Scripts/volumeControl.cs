using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class volumeControl : MonoBehaviour
{
    [SerializeField] string volParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    [SerializeField] private Toggle toggle;
    private bool disableToggleEvent;

    private void Awake()
    {
        slider.onValueChanged.AddListener(SliderValueChanged);
        toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void ToggleValueChanged(bool enableSound)
    {
        if (disableToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            slider.value = .8f;
        }
        else
        {
            slider.value = slider.minValue;
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volParameter, slider.value);
    }

    private void SliderValueChanged(float value)
    {
        mixer.SetFloat(volParameter, MathF.Log10(value) * multiplier);
        disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volParameter, slider.value);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
