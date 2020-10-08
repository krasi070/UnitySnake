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
