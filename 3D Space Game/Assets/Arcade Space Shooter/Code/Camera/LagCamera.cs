using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <sumamry>
/// Adds a slight lag to the camera rotation to make the third person camera more interesting. Requires that it starts parented to something in order to follow it correctly
/// </sumamry>
[RequireComponent(typeof(Camera))]
public class LagCamera : MonoBehaviour
{
    [Tooltip("Speed at which the camera rotates (uses slerp for rotation)")]
    public float rotateSpeed = 90.0f;

    [Tooltip("if the parented object is using FixedUpdate for movement, check this box for smoother movement")]
    public bool usedFixedUpdate = true;

    private Transform target;
    private Vector3 startOffset;

    private void Start()
    {
        target = transform.parent;

        if (target == null)
            Debug.LogWarning(name + ": Lag Camera will not function correctly without target.");
        if (transform.parent == null)
            Debug.LogWarning(name + ": Lag Camera will not function correctly without a parent to derive the initial offset from.");

        startOffset = transform.localPosition;
        transform.SetParent(null);
    }

    private void Update()
    {
        if (!usedFixedUpdate)
            UpdateCamera();
    }

    private void FixedUpdate()
    {
        if (usedFixedUpdate)
            UpdateCamera();
    }

    private void UpdateCamera()
    {
        if (target != null)
        {
            transform.position = target.TransformPoint(startOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
        }
    }
}