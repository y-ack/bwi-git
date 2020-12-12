using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;
    public InputField volumeInput;

    public void SetLevel(float sliderValue)
	{
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        volumeInput.text = ((int)(sliderValue * 100)).ToString();
	}

    public void setVolumeInput()
    { 

        int vInput = int.Parse(volumeInput.text);

        if (vInput > 100)
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(100/100) * 20);
            volumeSlider.value = 1;
            volumeInput.text = 100.ToString();
        } else if (vInput < 0)
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(0/100) * 20);
            volumeSlider.value = 0;
            volumeInput.text = 0.ToString();
        }
        else
        {
            mixer.SetFloat("MusicVol", Mathf.Log10(vInput/100) * 20);
            volumeSlider.value = (vInput/100f);
        }
    }

    


}
