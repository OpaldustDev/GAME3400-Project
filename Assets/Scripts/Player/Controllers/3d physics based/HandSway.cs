using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSway : MonoBehaviour
{
    [SerializeField] private PlayerStateController playerController;
    private bool active = true;
    [Tooltip("true for 3d based, false for UI based")]
    [SerializeField] private bool is3dBased = true;
    private RectTransform rectTransform;
    private Transform hands;
    public float swaySpeed = 2.0f;
    public float swayAmount = 1.0f;
    public float verticalSwayFactor = 2.0f; // Adjust as needed
    public float verticalLagAmount = 0.1f; // Adjust as needed

    private float timeCounter = 0.0f;

    private void OnEnable()
    {
        //GameController.GameInitialize += Deactivate;
        //GameController.GameStart += Activate;
        //GameController.GameStop += Deactivate;
    }

    private void OnDisable()
    {
        //GameController.GameInitialize -= Deactivate;
        //GameController.GameStart -= Activate;
        //GameController.GameStop -= Deactivate;
    }

    private void Awake()
    {
        if (is3dBased)
        {
            hands = GetComponent<Transform>();
        } else
        {
            rectTransform = GetComponent<RectTransform>();

        }
    }
    
    private void Deactivate()
    {
        active = false;
    }

    private void Activate()
    {
        active= true;
    }


    private void Update()
    {
        if (!active) { return; }
        float playerSpeed = playerController.GetSpeed();
        float verticalVelocity = playerController.GetVerticalVelocity();

        // Calculate horizontal sway based on non-vertical velocity
        float xOffset = Mathf.Sin(timeCounter) * swayAmount * Mathf.Max(0, playerSpeed);

        // Calculate vertical bobbing based on vertical velocity
        float yOffset = Mathf.Sin(timeCounter * 2) * swayAmount * verticalSwayFactor;

        // Dampen the vertical bobbing as the vertical velocity moves away from 0
        float dampingFactor = 1 - Mathf.Abs(verticalVelocity); // Damping factor based on vertical velocity
        dampingFactor = Mathf.Clamp01(dampingFactor); // Ensure the damping factor is in the [0, 1] range
        yOffset *= dampingFactor;

        // Apply vertical lag based on vertical velocity
        float verticalLag = verticalVelocity * verticalLagAmount;
        yOffset -= verticalLag;

        if(is3dBased)
        {
            Vector3 newPos = new Vector3(xOffset, yOffset);
            hands.localPosition = newPos;
        } else
        {
            rectTransform.anchoredPosition = new Vector2(xOffset, yOffset);

        }

        timeCounter += Time.deltaTime * swaySpeed;
    }
}
