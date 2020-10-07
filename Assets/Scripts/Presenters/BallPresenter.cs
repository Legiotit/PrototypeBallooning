using UnityEngine;

public class BallPresenter : MonoBehaviour
{
    [SerializeField]
    private BallView ballView = null;

    private BallModel ballModel;

    private void Start()
    {
        ballModel.OnPositionXChanged += ballView.SetPositionX;
        ballModel.OnPositionYChanged += ballView.SetPositionY;
        ballModel.OnLevelChanged += ballView.CreateTrack;
        ballModel.OnItemsGenerated += ballView.CreateItems;
        ballModel.OnSizeChanged += ballView.SetSize;
        ballModel.OnItemDestroyed += ballView.DestroyItem;

        ballView.OnLeftPressed += ballModel.SetNextMoveLeft; ;
        ballView.OnRightPressed += ballModel.SetNextMoveRight; ;
        ballView.OnCollideItem += ballModel.CollideItem;

        ballModel.SetLevel(0);
    }

    private void OnDestroy()
    {
        ballModel.OnPositionXChanged -= ballView.SetPositionX;
        ballModel.OnPositionYChanged -= ballView.SetPositionY;
        ballModel.OnLevelChanged -= ballView.CreateTrack;
        ballModel.OnItemsGenerated -= ballView.CreateItems;
        ballModel.OnSizeChanged -= ballView.SetSize;
        ballModel.OnItemDestroyed -= ballView.DestroyItem;

        ballView.OnLeftPressed -= ballModel.SetNextMoveLeft;
        ballView.OnRightPressed -= ballModel.SetNextMoveRight;
        ballView.OnCollideItem -= ballModel.CollideItem;
    }

    private void Update()
    {
        ballModel.UpdateBall();
    }

    public void SetModel(BallModel ballModel)
    {
        this.ballModel = ballModel;
    }
}