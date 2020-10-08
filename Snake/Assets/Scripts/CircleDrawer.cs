using System.Collections;
using UnityEngine;

// 2D circle drawing code taken from this video https://www.youtube.com/watch?v=BoDwchoM9Ic&ab_channel=KristerCederlund
[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.15f;
    public float radius;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void StartEffect()
    {
        StartCoroutine(EnlargeCircle());
    }

    private IEnumerator EnlargeCircle()
    {
        float speed = 20f;
        radius = 0f;

        while (radius < Camera.main.orthographicSize * 4)
        {
            radius += speed * Time.deltaTime;
            Draw();

            yield return null;
        }

        Destroy(gameObject);
    }

    private void Draw()
    {
        _lineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        _lineRenderer.positionCount = vertexCount;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta) + transform.position.x, radius * Mathf.Sin(theta) + transform.position.y, 0f);
            _lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}