using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBlock : MonoBehaviour
{
    private bool _hidden;
    private Vector2Int _coordinates;

    public bool Hidden
    {
        get
        {
            return _hidden;
        }
        set
        {
            if (SpriteRenderer != null)
            {
                SpriteRenderer.enabled = !value;
            }

            if (ShadowSpriteRenderer != null)
            {
                ShadowSpriteRenderer.enabled = !value;
            }

            _hidden = value;
        }
    }

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

    public SpriteRenderer SpriteRenderer { get; set; }

    public SpriteRenderer ShadowSpriteRenderer { get; set; }

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
            if (!Hidden)
            {
                timer += Time.deltaTime;
                transform.localScale = Vector3.one - Vector3.Lerp(Vector3.zero, vectorAmount, Mathf.Abs(Mathf.Sin(timer * speed)));
            }
            
            yield return null;
        }
    }
}
