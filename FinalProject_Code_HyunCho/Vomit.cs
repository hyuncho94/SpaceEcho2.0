using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;

public class Vomit : MonoBehaviour
{
    public Modular3DText letter;
    public float upwardForce = 1f;
    public float destroyTime = 5f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        }

        Destroy(gameObject, destroyTime);
    }

    public void newTypeOneChar(string character)
    {
        if (letter != null)
        {
            letter.UpdateText(character);
        }
    }
}
