using UnityEngine;

public class BallModelManager : MonoBehaviour
{
    [SerializeField]
    private BallModel ballModel = null;

    [SerializeField]
    private BallPresenter ballPresenter = null;

    [SerializeField]
    private UIPresenter uiPresenter = null;

    private void Awake()
    {
        ballModel = new BallModel(0.00005f, 0.01f);
        ballPresenter.SetModel(ballModel);
        uiPresenter.SetModel(ballModel);
    }
}
