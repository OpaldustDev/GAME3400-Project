using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BossFollow : MonoBehaviour
{
    [SerializeField] Transform targetPlayer;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float minDist = 4f;
    // Start is called before the first frame update
    void Start()
    {
        if(null == targetPlayer) {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // make reaper look at player
        transform.LookAt(targetPlayer);
        float distance = Vector3.Distance(gameObject.transform.position, targetPlayer.position);
        if(distance <= minDist) {
            // move the reaper towards the player every frame
            transform.position = Vector3.MoveTowards
                (transform.position, targetPlayer.position, moveSpeed * Time.deltaTime);
        }
    }
}
