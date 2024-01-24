using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBalloon : MonoBehaviour
{
    public ParticleSystem popping;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // change this so that if there is a collision, the particle plays
        // and the balloon gets destroyed
        if(Input.anyKeyDown) {
            popping.Play();
            Destroy(gameObject, 1);
        }
    }
}
