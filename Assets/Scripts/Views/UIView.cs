using UnityEngine.UI;
using UnityEngine;
using System;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private Text score = null;

    [SerializeField]
    private DownButton leftButton = null;

    [SerializeField]
    private DownButton rightButton = null;

    [SerializeField]
    private RectTransform restartPanel = null;

    [SerializeField]
    private RectTransform endPanel = null;

    [SerializeField]
    private Button restartButton = null;

    [SerializeField]
    private Button endButton = null;

    [SerializeField]
    private Text restartScore = null;

    [SerializeField]
    private Text endScore = null;

    private int lastScore;

    public event Action OnLeftPress;

    public event Action OnRightPress;

    public event Action OnRestartPress;

    public event Action OnEndPress;

    private void Start()
    {
        leftButton.OnButtonDown += OnLeftPress;
        rightButton.OnButtonDown += OnRightPress;

        restartButton.onClick.AddListener(() => OnRestartPress?.Invoke());
        endButton.onClick.AddListener(() => OnEndPress?.Invoke());
    }

    private void OnDestroy()
    {
        leftButton.OnButtonDown -= OnLeftPress;
        rightButton.OnButtonDown -= OnRightPress;

        restartButton.onClick.RemoveAllListeners();
        endButton.onClick.RemoveAllListeners();
    }

    public void SetScore(int score)
    {
        this.score.text = score.ToString();
        lastScore = score;
    }

    public void ShowRestartPanel(bool b)
    {
        if (b)
        {
            restartPanel.gameObject.SetActive(true);
            restartScore.text = lastScore.ToString();
        }
        else
        {
            restartPanel.gameObject.SetActive(false);
        }
    }

    public void ShowEndPanel(bool b)
    {
        if (b)
        {
            endPanel.gameObject.SetActive(true);
            endScore.text = lastScore.ToString();
        }
        else
        {
            endPanel.gameObject.SetActive(false);
        }
    }
}
