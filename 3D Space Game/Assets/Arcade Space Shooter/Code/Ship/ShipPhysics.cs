using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies linear and angular forces to a ship.
/// </summary>
public class ShipPhysics : MonoBehaviour
{
    [Tooltip("X: Lateral thrust\nY: Vertical Thrust\nZ: Longitudinal Thrust")]
    public Vector3 linearForce = new Vector3(100.0f, 100.0f, 100.0f);

    [Tooltip("X: Pitch\nY: Yaw\nZ: Roll")]
    public Vector3 angularForce = new Vector3(100.0f, 100.0f, 100.0f);

    [Range(0.0f, 1.0f)]
    [Tooltip("Multiplier for longitudinal thrust when reverse thrust is required")]
    public float reverseMultiplier = 1.0f;

    [Tooltip("Multiplier for all forces. Can be used to keep force numbers smaller and more readable")]
    public float forceMultiplier = 100.0f;
    
    public Rigidbody Rigidbody { get { return rbody; } }

    private Vector3 appliedLinearForce = Vector3.zero;
    private Vector3 appliedAngularForce = Vector3.zero;

    private Rigidbody rbody;

    // Keep a refference to the ship this is attached to just in case
    private Ship ship;

    // Initializer
    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        if (rbody == null)
            Debug.LogWarning(name + ": ShipPhysics has no rigidbody");

        ship = GetComponent<Ship>();
    }

    void FixedUpdate()
    {
        if (rbody != null)
        {
            rbody.AddRelativeForce(appliedLinearForce * forceMultiplier, ForceMode.Force);
            rbody.AddRelativeTorque(appliedAngularForce * forceMultiplier, ForceMode.Force);
        }
    }

    /// <summary>
    /// Sets the input for how much of the linearforce and angularforce are applied to the ship. each component of the input vectors is assumed to be scaled from -1 to 1, but is not clamped.
    /// </summary>
    public void SetPhysicsInput(Vector3 linearInput, Vector3 AngularInput)
    {
        appliedLinearForce = MultiplyByComponent(linearInput, linearForce);
        appliedAngularForce = MultiplyByComponent(AngularInput, angularForce);
    }

    /// <summary>
    /// Returns a Vector3 where each component of Vector A is multiplied by the equivalent component of Vector B.
    /// </summary>
    private Vector3 MultiplyByComponent(Vector3 a, Vector3 b)
    {
        Vector3 ret;

        ret.x = a.x * b.x;
        ret.y = a.y * b.y;
        ret.z = a.z * b.z;

        return ret;
    }
}