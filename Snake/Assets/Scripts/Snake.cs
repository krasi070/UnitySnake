using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // The speed is in units per second
    public float speed;

    private Vector2Int _direction;

    private float _waitTime;
    private float _timer;

    // The head is the last element in the list
    private List<BodyPart> _bodyParts;

    public BodyPart Head { get; private set; }

    public bool Active { get; set; }

    public int Multiplier { get; set; }

    private void Start()
    {
        _bodyParts = new List<BodyPart>();
        Head = GetComponentInChildren<BodyPart>();
        Head.Coordinates = Vector2Int.zero;
        _bodyParts.Add(Head);
        _direction = Vector2Int.right;

        _waitTime = 1 / speed;
        _timer = 0;
        Active = true;
    }

    private void Update()
    {
        if (Active)
        {
            _timer += Time.deltaTime;
            CheckForInput();

            if (_timer >= _waitTime)
            {
                Move();
            }
        }
    }

    public void InitBodyPart()
    {
        GameObject newBodyPart = new GameObject("BodyPart");
        newBodyPart.transform.SetParent(transform);

        SpriteRenderer renderer = newBodyPart.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Sprites/white_sprite");
        renderer.color = Head.GetComponent<SpriteRenderer>().color;

        BodyPart bodyPart = newBodyPart.AddComponent<BodyPart>();
        bodyPart.Next = Head;
        bodyPart.IsNew = true;
        bodyPart.Coordinates = Head.Coordinates;
        _bodyParts.Add(bodyPart);
        AddShadowToBodyPart(bodyPart);
        Head = bodyPart;
    }

    public IList<Vector2Int> GetFreeCoordinates(int rows, int columns)
    {
        IList<Vector2Int> freeCoordinates = new List<Vector2Int>();
        bool[,] isSnake = new bool[rows, columns];

        BodyPart currBodyPart = Head;

        while (currBodyPart != null)
        {
            isSnake[currBodyPart.Coordinates.y, currBodyPart.Coordinates.x] = true;
            currBodyPart = currBodyPart.Next;
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (!isSnake[row, col])
                {
                    freeCoordinates.Add(new Vector2Int(col, row));
                }
            }
        }

        return freeCoordinates;
    }

    public void ResetValues()
    {
        BodyPart currBodyPart = Head.Next;

        while (currBodyPart != null)
        {
            GameObject currObj = currBodyPart.gameObject;
            currBodyPart = currBodyPart.Next;
            Destroy(currObj);
        }

        Head.Next = null;
        _direction = Vector2Int.right;
        _timer = 0;
        _bodyParts = new List<BodyPart>();
        _bodyParts.Add(Head);
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
        _waitTime = 1 / speed;
        _timer = 0;
    }

    public void CutTail(int amountToCut)
    {
        if (amountToCut < _bodyParts.Count)
        {
            _bodyParts[amountToCut].Next = null;

            for (int i = 0; i < amountToCut; i++)
            {
                Destroy(_bodyParts[i].gameObject);
            }

            _bodyParts.RemoveRange(0, amountToCut);
        }
    }

    private void Move()
    {
        Board board = GetComponentInParent<Board>();

        Vector2Int newCoordinates = Head.Coordinates + _direction;

        if (board.wrapAround)
        {
            newCoordinates = new Vector2Int(
                (board.columns + Head.Coordinates.x + _direction.x) % board.columns,
                (board.rows + Head.Coordinates.y + _direction.y) % board.rows);
        }

        if (!CheckForFutureCollision(newCoordinates))
        {
            Head.Move(newCoordinates);
        }
        else
        {
            Head.Collided = true;
            Head.StartShaking();
        }
        
        _timer = 0;
    }

    private bool CheckForFutureCollision(Vector2Int nextCoordinates)
    {
        Board board = GetComponentInParent<Board>();

        if (nextCoordinates.x < 0 || nextCoordinates.x >= board.columns ||
            nextCoordinates.y < 0 || nextCoordinates.y >= board.rows)
        {
            return true;
        }

        if (!Head.IsNew)
        {
            foreach (BodyPart bodyPart in _bodyParts)
            {
                if ((nextCoordinates - bodyPart.Coordinates).magnitude == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void AddShadowToBodyPart(BodyPart bodyPart)
    {
        GameObject shadow = new GameObject("Shadow");
        shadow.transform.position = bodyPart.transform.position + new Vector3(0.125f, -0.125f, 1f);
        shadow.transform.SetParent(bodyPart.transform);

        SpriteRenderer renderer = shadow.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Sprites/white_sprite");
        renderer.color = new Color32(30, 30, 30, 255);
    }

    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ChangeDirection(KeyCode.UpArrow);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChangeDirection(KeyCode.RightArrow);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ChangeDirection(KeyCode.DownArrow);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChangeDirection(KeyCode.LeftArrow);
        }
    }

    private void ChangeDirection(KeyCode input)
    {
        Vector2Int oldDirection = _direction;

        switch (input)
        {
            case KeyCode.UpArrow:
                _direction = Vector2Int.up;
                break;
            case KeyCode.RightArrow:
                _direction = Vector2Int.right;
                break;
            case KeyCode.DownArrow:
                _direction = Vector2Int.down;
                break;
            case KeyCode.LeftArrow:
                _direction = Vector2Int.left;
                break;
            default:
                Debug.LogError($"Received unexpected input: {input.ToString()}!");
                break;
        }

        if ((oldDirection - _direction).magnitude != 0 && (oldDirection - _direction).magnitude != 2)
        {
            Move();
        }
        else
        {
            _direction = oldDirection;
        }
    }
}