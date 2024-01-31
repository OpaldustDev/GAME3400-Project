using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultPlayerMovementStats", menuName = "ScriptableObjects/PlayerMovementStats", order = 1)]
public class PlayerMovementStatsSO : ScriptableObject
{
    public float maxSpeed = 5f;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float jumpForce = 3f;
    public int jumpCount = 1;
    public float coyoteTime = 0.5f;
    public float maxFallingVelocity;

}
