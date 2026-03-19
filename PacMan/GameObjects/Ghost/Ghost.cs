using Framework.Engine;
using System;

class Ghost : GameObject
{
    readonly BFSHelper BFSHelper = new();

    public (int x, int y)[] directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };

    public (int x, int y) Position;

    private const float k_MoveInterval = 0.3f;
    private const float k_FrightenedMoveInterval = 0.5f;
    private const float k_GoingHomeMoveInterval = 0.2f;

    private float currentMoveInterval;

    protected (int x, int y) _nextDirection;
    private float _moveTimer;

    public bool frightened;
    public bool goingHome;

    protected Ghost(Scene scene) : base(scene)
    {
        currentMoveInterval = k_MoveInterval;
    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;

        if (_moveTimer > currentMoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }
    }

    public virtual void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)    // 팩맨 위치가 변할 때마다 위치를 받아 경로 재계산 (가상함수)
    {
    }

    public virtual void Move()
    {
        Position = (x: Position.x + _nextDirection.x, y: Position.y + _nextDirection.y);
        if (IsHit() && !frightened) GameScene.isGameOver = true;
    }

    public bool IsHit() => MapManager.MapTile[Position.y, Position.x] == Tile.PacMan;

    public virtual void FrightenedOn()
    {
        frightened = true;
        currentMoveInterval = k_FrightenedMoveInterval;
    }
    public virtual void FrightenedOff()
    {
        frightened = false;
        currentMoveInterval = k_MoveInterval;
    }

    public virtual void GoingHomeOn()
    {
        goingHome = true;
        currentMoveInterval = k_GoingHomeMoveInterval;
    }

    public override void Draw(ScreenBuffer buffer) => buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩');    
}