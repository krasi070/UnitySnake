using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector2Int _coordinates;

    public Vector2Int Coordinates
    {
        get
        {
            return _coordinates;
        }
        set
        {
            transform.position = new Vector2(value.x, value.y);
            _coordinates = value;
        }
    }

    public void ChangePosition(IList<Vector2Int> availablePositions)
    {
        Coordinates = availablePositions[Random.Range(0, availablePositions.Count)];
    }

    public void StartPulsating()
    {
        StartCoroutine(Pulsate());
    }

    private IEnumerator Pulsate()
    {
        float timer = 0f;
        float amount = 0.25f;
        float speed = 8f;
        Vector3 vectorAmount = new Vector3(amount, amount, amount);

        while (true)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one - Vector3.Lerp(Vector3.zero, vectorAmount, Mathf.Abs(Mathf.Sin(timer * speed)));

            yield return null;
        }
    }
}