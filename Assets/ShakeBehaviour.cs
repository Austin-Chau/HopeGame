using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Copy and pasted directly from https://medium.com/@mattThousand/basic-2d-screen-shake-in-unity-9c27b56b516
/// Just a placeholder to test out effects
/// </summary>
public class ShakeBehaviour : MonoBehaviour
{

    // Desired duration of the shake effect
    private float shakeDuration = .05f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.5f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 20.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;
    // Start is called before the first frame update
    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        initialPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
    public void TriggerShake()
    {
        shakeDuration = 2.0f;
    }
}
