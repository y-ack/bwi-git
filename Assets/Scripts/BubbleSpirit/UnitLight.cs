using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class UnitLight : MonoBehaviour
{
    private Light2D myLight;

    
    public float eachFadeTime;
    public float fadeWaitTime;
    // Start is called before the first frame update
    void Start()
    {
         eachFadeTime = Random.Range(2f,4f);
         fadeWaitTime = Random.Range(3f,5f);
        StartCoroutine(fadeInAndOutRepeat(myLight, eachFadeTime, fadeWaitTime));
    }
        //Fade in and out forever
    IEnumerator fadeInAndOutRepeat(Light2D lightToFade, float duration, float waitTime)
    {
        WaitForSeconds waitForXSec = new WaitForSeconds(waitTime);

        while (true)
        {
            //Fade out
            yield return fadeInAndOut(lightToFade, false, duration);

            //Wait
            yield return waitForXSec;

            //Fade-in 
            yield return fadeInAndOut(lightToFade, true, duration);
        }
    }

    IEnumerator fadeInAndOut(Light2D lightToFade, bool fadeIn, float duration)
    {
        float minLuminosity = Random.Range(0.5f,0.8f); // min intensity
        float maxLuminosity = Random.Range(1f,1.5f); // max intensity

        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;

        if (fadeIn)
        {
            a = minLuminosity;
            b = maxLuminosity;
        }
        else
        {
            a = maxLuminosity;
            b = minLuminosity;
        }

        float currentIntensity = lightToFade.intensity;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lightToFade.intensity = Mathf.Lerp(a, b, counter / duration);

            yield return null;
        }
    }


}
