using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayInvis : MonoBehaviour
{
    [SerializeField] ParticleSystem invisibility;
    [SerializeField] Slider slider;
    bool activatible = true;
    public float lerpDuration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKey(KeyCode.Space)) {
            invisibility.Play();
        }*/
    }
    public void PlayInvisible()
    {
        if (activatible == false) { return; }
        StartCoroutine(LerpSliderToZero());
        invisibility.Play();
        activatible = false;
    }

    IEnumerator LerpSliderToZero()
    {
        float elapsedTime = 0f;
        float startValue = slider.value;
        float endValue = 0f;

        while (elapsedTime < lerpDuration)
        {
            slider.value = Mathf.Lerp(startValue, endValue, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        slider.value = endValue;
        yield return new WaitForSeconds(3f);
        activatible = true;
        slider.value = startValue;
    }
}
