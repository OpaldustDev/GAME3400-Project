using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] GameObject dart;
    Camera cam;
    float t = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
        cam = Camera.main;
        Debug.Log(cam);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (Input.GetMouseButton(0) && t > 0.5f)
        {
            GameObject temp = Instantiate(dart, this.transform.position, this.transform.rotation);
            temp.GetComponent<Rigidbody>().AddForce(cam.transform.forward * 14, ForceMode.Impulse);
            t = 0;
        }
    }
}
