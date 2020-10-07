using System;
using System.Collections.Generic;
using UnityEngine;

public class BallModel
{
    private float _finishSize = 0.03f;  
    public float FinishSize {get; set;}

    private float _sizeReductionRate = 0.0005f;
    public float SizeReductionRate { get; set; }

    private float _scoreFinishScale = 10000f;
    public float ScoreFinishScale { get; set; }

    private float _speedFinishScale = 0.2f;
    public float SpeedFinishScale { get; set; }

    public StatusGame _status { get; set; }
    public StatusGame Status
    {
        get => _status;
        set
        {
            _status = value;
            switch (value)
            {
                case StatusGame.Game:
                    OnShowEndPanel?.Invoke(false);
                    OnShowRestartPanel?.Invoke(false);
                    break;
                case StatusGame.Pause:
                    break;
                case StatusGame.End:
                    OnShowEndPanel?.Invoke(true);
                    OnShowRestartPanel?.Invoke(false);
                    break;
                case StatusGame.Death:
                    OnShowRestartPanel?.Invoke(true);
                    OnShowEndPanel?.Invoke(false);
                    break;
            }
        }
    }

    private int _level = 0;
    public int Level { get => _level; private set => _level = value; }

    private float _positionX = 0.01f;
    public float PositionX
    {   
        get => _positionX;
        private set
        {
            _positionX = value;
            OnPositionXChanged?.Invoke(value);
        }
    }

    private float _positionY = 0;
    public float PositionY
    {
        get => _positionY;
        private set
        {
            _positionY = value;
            OnPositionYChanged?.Invoke(value);
        }
    }

    private float _ballSize = 0.5f;
    public float BallSize 
    { 
        get => _ballSize;
        set
        {
            _ballSize = value;
            if(_ballSize > MaxBallSize)
            {
                _ballSize = MaxBallSize;
            }

            if (_ballSize < MinBallSize)
            {
                _ballSize = MinBallSize;
            }

            OnSizeChanged?.Invoke(_ballSize);
        } 
    }

    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(value);
        }
    }

    private float _speedX;
    public float SpeedX { get => _speedX; set => _speedX = value; }

    private float _speedY;
    public float SpeedY { get => _speedY; set => _speedY = value; }

    private float _sizeBonus = 0.03f;
    public float SizeBonus { get => _sizeBonus; set => _sizeBonus = value; }

    private int _blueItemScore = 20;
    public int BlueItemScore { get => _blueItemScore; set => _blueItemScore = value; }

    private int _redItemScore = 5;
    public int RedItemScore { get => _redItemScore; set => _redItemScore = value; }

    private float _maxBallSize = 1;
    public float MaxBallSize { get => _maxBallSize; set => _maxBallSize = value; }

    private float _minBallSize = 0.1f;
    public float MinBallSize { get => _minBallSize; set => _minBallSize = value; }

    private bool isMovingLeft, isMovingRight;

    private List<Item> items = new List<Item>();

    private int rowCount = 100;

    private float itemProbability = 0.5f;

    public event Action<int> OnLevelChanged;

    public event Action<float> OnPositionXChanged;

    public event Action<float> OnPositionYChanged;

    public event Action<List<Item>> OnItemsGenerated;

    public event Action<float> OnSizeChanged;

    public event Action<int> OnScoreChanged;

    public event Action<bool> OnShowRestartPanel;

    public event Action<bool> OnShowEndPanel;

    public event Action<Item> OnItemDestroyed;

    public BallModel(float speedX, float speedY)
    {
        SpeedX = speedX;
        SpeedY = speedY;
        FinishSize = _finishSize;
        SizeReductionRate = _sizeReductionRate;
        ScoreFinishScale = _scoreFinishScale;
        SpeedFinishScale = _speedFinishScale;
        Status = StatusGame.Game;
    }

    public void SetLevel(int level)
    {
        Level = level;
        OnLevelChanged?.Invoke(Level);
        PositionX = 0.01f;
        PositionY = 0;
        GenerateItems();
        Status = StatusGame.Game;
        BallSize = 0.5f;
        Score = 0;
    }

    private void GenerateItems()
    {
        items.Clear();
        System.Random rand = new System.Random();
        Array allTypes = Enum.GetValues(typeof(ItemType));

        for (int row = 0; row < rowCount; row++)
        {
            float itemPosX = ((1 - FinishSize * 1.5f - PositionX) / rowCount) * (row + 1) + PositionX;
            int numTraps = 0;
            for (int count = 0; count < 3; count++)
            {
                if (rand.NextDouble() > itemProbability)
                {
                    float itemPosY = 0.5f * (count + 1) - 1;
                    ItemType itemType = (ItemType)allTypes.GetValue(rand.Next(allTypes.Length));
                    if (itemType == ItemType.Mine)
                    {
                        numTraps += 1;
                    }
                    if (numTraps < 2)
                    {
                        items.Add(new Item(itemType, itemPosX, itemPosY));
                    }
                }
            }
        }

        OnItemsGenerated?.Invoke(items);
    }

    public void CollideItem(Item item)
    {
        switch(item.itemType)
        {
            case (ItemType.Positive):
                Score += BlueItemScore;
                BallSize += SizeBonus;
                OnItemDestroyed?.Invoke(item);
                break;
            case (ItemType.Negative):
                Score += RedItemScore;
                BallSize -= SizeBonus;
                OnItemDestroyed?.Invoke(item);
                break;
            case (ItemType.Mine):
                OnItemDestroyed?.Invoke(item);
                Status = StatusGame.Death;
                break;
        }
    }

    public void SetNextMoveLeft()
    {
        isMovingLeft = true;
    }

    public void SetNextMoveRight()
    {
        isMovingRight = true;
    }

    public void UpdateBall()
    {
        if (Status == StatusGame.Game)
        {
            float newPositionY = PositionY;
            float newPositionX = PositionX;

            if (newPositionX > 1 - FinishSize)
            {
                if (BallSize > 0.2f)
                {
                    newPositionX = newPositionX > 1 ? 1 : PositionX + SpeedX * SpeedFinishScale;
                    BallSize -= SizeReductionRate;
                }
                else
                {
                    Score += (int)((newPositionX + FinishSize - 1) * ScoreFinishScale);
                    Status = StatusGame.End;
                }
            }
            else
            {
                newPositionX += SpeedX;
                if (isMovingLeft != isMovingRight)
                {
                    float moveDirection = isMovingRight ? 1 : -1;
                    newPositionY = PositionY + moveDirection * SpeedY;

                    if (newPositionY < -1)
                    {
                        newPositionY = -1;
                    }

                    if (newPositionY > 1)
                    {
                        newPositionY = 1;
                    }
                }
            }
            PositionX = newPositionX;
            PositionY = newPositionY;

            isMovingLeft = false;
            isMovingRight = false;
        }
    }

    public void RestartGame()
    {
        Status = StatusGame.Game;
        SetLevel(Level);
    }

    public void NextLevel()
    {
        Status = StatusGame.Game;
        SetLevel(Level + 1);
    }
}

public enum StatusGame 
{
    Game,
    Pause,
    End,
    Death
}
