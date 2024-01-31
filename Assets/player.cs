using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    //GAME3400Project1 inputs = new GAME3400Project1 ();

    [SerializeField] GameObject dart;
    Camera cam;
    CinemachineBrain brain;
    float t = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
        cam = Camera.main;

        Debug.Log(cam);
        //inputs.Player.Fire.performed += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        /*if (inputs.Player.Fire. && t > 0.5f)
        {
            GameObject temp = Instantiate(dart, this.transform.position, this.transform.rotation);
            temp.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 14, ForceMode.Impulse);
            t = 0;
        }*/
    }

    /*void Shoot(InputAction.CallbackContext obj)
    {
        if (t < 0.5f) { return; }
        GameObject temp = Instantiate(dart, this.transform.position, this.transform.rotation);
        temp.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 14, ForceMode.Impulse);
        t = 0;
    }*/

    public void OnClick()
    {
        if (t < 0.5f) { return; }

        GameObject temp = Instantiate(dart, this.transform.position, cam.transform.rotation);
        Debug.Log(-cam.gameObject.transform.forward);
        temp.GetComponent<Rigidbody>().AddForce(-cam.gameObject.transform.forward * 14, ForceMode.Impulse);
        t = 0;
    }
}
