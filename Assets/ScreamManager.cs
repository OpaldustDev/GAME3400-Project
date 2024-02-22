using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ScreamManager : MonoBehaviour
{
    public float delayTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().PlayDelayed(delayTime);
    }

}
