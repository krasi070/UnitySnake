using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    private Text _scoreTextField;

    private int _score;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            _scoreTextField.text = _score.ToString();
        }
    }

    private void Start()
    {
        _scoreTextField = GetComponentInChildren<Text>();
    }
}