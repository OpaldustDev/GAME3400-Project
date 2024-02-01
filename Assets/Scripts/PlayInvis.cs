using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInvis : MonoBehaviour
{
    [SerializeField] ParticleSystem invisibility;
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
        invisibility.Play();
    }
}
