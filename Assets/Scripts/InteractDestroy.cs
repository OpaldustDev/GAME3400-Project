using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDestroy : MonoBehaviour, Interactables
{
    public void Interact()
    {
        Destroy(this.gameObject);   
    }
}
