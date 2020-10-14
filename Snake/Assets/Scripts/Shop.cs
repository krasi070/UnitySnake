using UnityEngine;

public class Shop : MonoBehaviour
{
    private Snake _snake;
    private Board _board;

    private void Start()
    {
        _board = GameObject.Find("Board").GetComponent<Board>();
        _snake = _board.GetComponentInChildren<Snake>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            IncreaseSnakeSpeed();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            IncreaseBoardSize(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            IncreaseBoardSize(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _snake.CutTail(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _snake.CutTail(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _snake.CutTail(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _snake.CutTail(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _snake.CutTail(5);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.InitMultiplierBlock();
        }
    }

    private void IncreaseSnakeSpeed()
    {
        _snake.UpdateSpeed(_snake.speed + 1);
        Debug.Log($"Speed: {_snake.speed - 4}");
    }

    private void IncreaseBoardSize(int rows, int cols)
    {
        _board.rows += rows;
        _board.columns += cols;
        _board.DeleteBoardElements();
        _board.InitBoard();
    }
}
