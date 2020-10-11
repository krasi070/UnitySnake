using System.Collections;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    private Vector2Int _coordinates;

    public BodyPart Next { get; set; }

    public bool IsNew { get; set; }

    public bool Collided { get; set; }

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

    public void Move(Vector2Int newCoordinates)
    {
        if (!IsNew)
        {
            Next?.Move(Coordinates);
        }
        else
        {
            IsNew = false;
        }

        Coordinates = newCoordinates;
    }

    public void StartShaking()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float speed = 1f;
        float amount = 2f;
        float duration = 0.5f;
        float timer = 0f;
        Vector3 startingPos = transform.position;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float x = startingPos.x + Time.deltaTime * speed * amount * Random.Range(-1, 2);
            float y = startingPos.y + Time.deltaTime * speed * amount * Random.Range(-1, 2);
            transform.position = new Vector3(x, y);

            yield return null;
        }

        transform.position = startingPos;
    }
}