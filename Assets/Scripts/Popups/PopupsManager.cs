using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupsManager : MonoBehaviour
{
    private string text = "";
    private char[] toDisplayText;
    private PopupEffect popupEffect;

    [SerializeField] private ElementFader powerupsUI;
    [SerializeField] private ElementFader popupUI;
    [SerializeField] private TextMeshProUGUI currentText;

    private Coroutine currentPopupSequence;

    private float textSpeed = 0.1f;
    private float endDelay = 6f;

    private void OnEnable()
    {
        Popup.PopupEvent += RecievePopupIntructions;
    }

    private void OnDisable()
    {
        Popup.PopupEvent -= RecievePopupIntructions;
    }

    private void Start()
    {
        popupUI.SetAlpha(0f);
    }

    private void RecievePopupIntructions(string text, PopupEffect effect)
    {
        currentText.text = "";
        toDisplayText = text.ToCharArray();
        popupEffect = effect;

        if (currentPopupSequence != null)
        {
            StopCoroutine(currentPopupSequence);
        }
        currentPopupSequence = StartCoroutine(ProcessPopup());
    }

    private IEnumerator ProcessPopup()
    {
        powerupsUI.MoveElement(-1, 165f, 2);
        yield return new WaitForSeconds(1);
        popupUI.FadeIn();

        foreach (char character in toDisplayText)
        {
            text = text + character;
            currentText.text = text;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(endDelay);
        popupUI.FadeOut();
        yield return new WaitForSeconds(1);
        powerupsUI.MoveElement(1, 165f, 2);
    }
}
