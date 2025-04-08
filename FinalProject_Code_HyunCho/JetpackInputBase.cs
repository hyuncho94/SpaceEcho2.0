using UnityEngine;
using System;

public abstract class JetpackInputBase : MonoBehaviour
{
    [SerializeField] protected float thrustMultiplier = 5f;
    protected Vector3 currentForce;
    public Vector3 CurrentForce => currentForce;

    public event Action<float> EvtThrustInputChanged = delegate { };
    public event Action<Vector3> EvtThrustChanged = delegate { };

    protected float currentThrustInput;

    protected void ThrustInputChanged(float newInput, Vector3 direction)
    {
        currentThrustInput = newInput;
        currentForce = direction * (newInput * thrustMultiplier);

        EvtThrustInputChanged.Invoke(newInput);
        EvtThrustChanged.Invoke(currentForce);
    }

    // Called by child class to apply input logic
    public abstract void ProcessInput();
}
