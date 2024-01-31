using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public static event Action<String, PopupEffect> PopupEvent;

    [SerializeField] private PopupDialogueSO dialogueSO;
    [SerializeField] private PopupEffect popupEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")){ return; }
        PopupEvent?.Invoke(dialogueSO.dialogue, null);
        this.gameObject.SetActive(false);
    }
}
