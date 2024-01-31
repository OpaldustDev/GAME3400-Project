using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float time = 3f;

    void Start()
    {
        Destroy(gameObject, time);
    }

}
