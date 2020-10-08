using UnityEngine;

// Code by Matt Buckley: https://medium.com/@mattThousand/basic-2d-screen-shake-in-unity-9c27b56b516
public class ShakeBehaviour : MonoBehaviour
{
    // Desired duration of the shake effect
    private float _shakeDuration = -100f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private float _shakeMagnitude = 0.0225f;
    private float _shakeMagnitudeMultiplier = 1f;

    // A measure of how quickly the shake effect should evaporate
    private float _dampingSpeed = 1f;

    // The initial position of the GameObject
    private Vector3 _initialPosition;

    private void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.localPosition = _initialPosition + Random.insideUnitSphere * _shakeMagnitude * _shakeMagnitudeMultiplier;

            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else if (_shakeDuration > -1)
        {
            _shakeDuration = -100f;
            transform.localPosition = _initialPosition;
        }
    }

    public void TriggerShake(float shakeMagnitudeMultiplier)
    {
        _shakeDuration = 0.275f;
        _shakeMagnitudeMultiplier = shakeMagnitudeMultiplier;
        _initialPosition = transform.localPosition;
    }
}