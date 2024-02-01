using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabPrize : MonoBehaviour
{
    [SerializeField] Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        //Debug.Log("Distance between " + gameObject.name + " and player " + distance);
        if(distance <= 4 && Input.GetKey(KeyCode.E)) {
            Debug.Log(gameObject.name + " has been grabbed by player");
            Destroy(gameObject);
        }
    }
}
