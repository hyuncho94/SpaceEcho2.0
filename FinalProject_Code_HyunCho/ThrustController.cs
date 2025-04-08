using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustController : MonoBehaviour
{
    [SerializeField] private Rigidbody targetRigidbody;
    [SerializeField] private bool isLocal = false;

    private JetpackInputBase leftInput;
    private JetpackInputBase rightInput;

    public void SetUp(JetpackInputBase left, JetpackInputBase right, bool local)
    {
        leftInput = left;
        rightInput = right;
        isLocal = local;
    }

    void FixedUpdate()
    {
        if (!isLocal || targetRigidbody == null) return;

        // Apply force based on inputs
        Vector3 force = Vector3.zero;
        if (leftInput != null) force += leftInput.CurrentForce;
        if (rightInput != null) force += rightInput.CurrentForce;

        targetRigidbody.AddForce(force, ForceMode.Acceleration);
    }
}
