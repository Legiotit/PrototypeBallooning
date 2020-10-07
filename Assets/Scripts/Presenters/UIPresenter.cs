using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    private BallModel ballModel;

    [SerializeField]
    private UIView uiView = null;

    private void Start()
    {
        uiView.OnLeftPress += ballModel.SetNextMoveLeft;
        uiView.OnRightPress += ballModel.SetNextMoveRight;
        uiView.OnRestartPress += ballModel.RestartGame;
        uiView.OnEndPress += ballModel.NextLevel;

        ballModel.OnScoreChanged += uiView.SetScore;
        ballModel.OnShowRestartPanel += uiView.ShowRestartPanel;
        ballModel.OnShowEndPanel += uiView.ShowEndPanel;
    }

    private void OnDestroy()
    {
        uiView.OnLeftPress -= ballModel.SetNextMoveLeft;
        uiView.OnRightPress -= ballModel.SetNextMoveRight;
        uiView.OnRestartPress -= ballModel.RestartGame;
        uiView.OnEndPress -= ballModel.NextLevel;

        ballModel.OnScoreChanged -= uiView.SetScore;
        ballModel.OnShowRestartPanel -= uiView.ShowRestartPanel;
        ballModel.OnShowEndPanel -= uiView.ShowEndPanel;
    }

    public void SetModel(BallModel ballModel)
    {
        this.ballModel = ballModel;
    }
}
