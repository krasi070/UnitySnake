using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public bool wrapAround;
    public int rows;
    public int columns;

    public Color boardColor;
    public Color backgroundColor;

    public ScoreTracker scoreTracker;
    public Text gameOverTextField;

    private Sprite _bg1Sprite;
    private Sprite _bg2Sprite;

    private float _tileSpriteHeight;
    private float _tileSpriteWidth;

    private Snake _snake;
    private Coin _coin;

    private void Start()
    {
        LoadTileSprites();
        InitBoard();
        LoadSnake();
        InitCoin();
    }

    private void Update()
    {
        CheckForCoinCollision();
        CheckForGameOver();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject snakePrefab = Resources.Load<GameObject>("Prefabs/Shop");
            GameObject snakeObj = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity);
        }

        if (gameOverTextField.IsActive() && Input.GetKeyDown(KeyCode.Space))
        {
            Reset();
        }
    }

    private void OnValidate()
    {
        foreach (Transform tile in transform)
        {
            if (tile.tag == "BoardTile")
            {
                tile.GetComponent<SpriteRenderer>().color = boardColor;
            }
        }

        Camera.main.backgroundColor = backgroundColor;
        gameOverTextField.color = backgroundColor;

        if (_snake != null)
        {
            foreach (Transform bodyPart in _snake.transform)
            {
                bodyPart.GetComponent<SpriteRenderer>().color = backgroundColor;
            }
        }
    }

    private void LoadTileSprites()
    {
        _bg1Sprite = Resources.Load<Sprite>("Sprites/bg1_sprite");
        _bg2Sprite = Resources.Load<Sprite>("Sprites/bg2_sprite");

        _tileSpriteHeight = _bg1Sprite.bounds.size.y;
        _tileSpriteWidth = _bg1Sprite.bounds.size.x;
    }

    private void LoadSnake()
    {
        GameObject snakePrefab = Resources.Load<GameObject>("Prefabs/Snake");
        GameObject snakeObj = Instantiate(snakePrefab, Vector3.zero, Quaternion.identity, transform);

        SpriteRenderer headRenderer = snakeObj.transform.GetComponentInChildren<SpriteRenderer>();
        headRenderer.color = backgroundColor;

        _snake = snakeObj.GetComponent<Snake>();
    }

    private void InitCoin()
    {
        GameObject coin = new GameObject("Coin");
        coin.transform.SetParent(transform);

        SpriteRenderer renderer = coin.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Sprites/white_sprite");
        renderer.color = new Color32(230, 215, 65, 255);

        GameObject coinShadow = new GameObject("Shadow");
        coinShadow.transform.position = coin.transform.position + new Vector3(0.125f, -0.125f, 1f);
        coinShadow.transform.SetParent(coin.transform);

        SpriteRenderer shadowRenderer = coinShadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = Resources.Load<Sprite>("Sprites/white_sprite");
        shadowRenderer.color = new Color32(30, 30, 30, 255);

        _coin = coin.AddComponent<Coin>();
        _coin.ChangePosition(_snake.GetFreeCoordinates(rows, columns));
        _coin.StartPulsating();
    }

    public void InitBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = new GameObject("Tile");
                tile.transform.position = new Vector3(col, row, transform.position.z);
                tile.tag = "BoardTile";
                tile.transform.SetParent(transform);

                SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();

                if ((row + col) % 2 == 0)
                {
                    renderer.sprite = _bg1Sprite;
                }
                else
                {
                    renderer.sprite = _bg2Sprite;
                }

                renderer.color = boardColor;
            }
        }

        InitShadow();
        Camera.main.transform.position = new Vector3((columns - _tileSpriteWidth) / 2, (rows - _tileSpriteHeight) / 2, Camera.main.transform.position.z);
        Camera.main.backgroundColor = backgroundColor;

        gameOverTextField.gameObject.SetActive(false);
        gameOverTextField.color = backgroundColor;
    }

    public void DeleteBoardElements()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "Player" && child.name != "Coin")
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void InitShadow()
    {
        GameObject tile = new GameObject("Shadow");
        tile.transform.localScale = new Vector3(columns, rows, 1);
        tile.transform.position = new Vector3((columns - _tileSpriteWidth) / 2 + 0.3f, (rows - _tileSpriteHeight) / 2 - 0.3f, transform.position.z + 1);
        tile.transform.SetParent(transform);

        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = _bg2Sprite;
        renderer.color = new Color32(30, 30, 30, 255);
    }

    private void CheckForCoinCollision()
    {
        if ((_coin.Coordinates - _snake.Head.Coordinates).magnitude == 0)
        {
            CoinGetEffect(_coin.transform.position);
            _snake.InitBodyPart();
            _coin.ChangePosition(_snake.GetFreeCoordinates(rows, columns));
            scoreTracker.Score++;
        }
    }

    private void CoinGetEffect(Vector3 pos)
    {
        GameObject circleEffect = Resources.Load<GameObject>("Prefabs/CircleEffect");
        GameObject ceInstance = Instantiate(circleEffect, pos, Quaternion.identity);
        ceInstance.GetComponent<CircleDrawer>().StartEffect();

        if (scoreTracker.Score > 5)
        {
            Camera.main.GetComponent<ShakeBehaviour>().TriggerShake(Mathf.Min(2, (scoreTracker.Score - 5) * 0.1f));
        }
    }

    private void CheckForGameOver()
    {
        if (_snake.Head.Collided)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _snake.Active = false;
        gameOverTextField.gameObject.SetActive(true);
    }

    private void Reset()
    {
        scoreTracker.Score = 0;
        gameOverTextField.gameObject.SetActive(false);
        _snake.Head.Collided = false;
        _snake.ResetValues();
        _snake.Head.Coordinates = Vector2Int.zero;
        _coin.ChangePosition(_snake.GetFreeCoordinates(rows, columns));
        _snake.Active = true;
    }
}