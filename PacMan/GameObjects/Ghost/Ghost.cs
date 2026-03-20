using Framework.Engine;
using System;

abstract class Ghost : GameObject
{
    protected BFSHelper bfs = new();
    protected Random rand = new();

    public (int x, int y)[] directions = { (0, -1), (-1, 0), (0, 1), (1, 0) };

    public (int x, int y) Position;

    protected const float k_MoveInterval = 0.3f;
    protected const float k_FrightenedMoveInterval = 0.5f;
    protected const float k_GoingHomeMoveInterval = 0.1f;
    protected float currentMoveInterval;

    protected const float k_FrightenedDuration = 10f;    

    protected (int x, int y) _nextDirection;
    
    protected float _moveTimer;
    protected float _frightenedTimer;

    protected ConsoleColor _color = ConsoleColor.Blue;

    public bool frightened;
    public bool goingHome;

    protected Ghost(Scene scene) : base(scene)
    {
        frightened = false;
        goingHome = false;

        currentMoveInterval = k_MoveInterval;

        GameScene.OnFrightenedMode += FrightenedOn;
        GameScene.OffFrightenedMode += FrightenedOff;
    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;
        if (frightened) _frightenedTimer += deltaTime;

        if (_moveTimer > currentMoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }

        if (_frightenedTimer > k_FrightenedDuration)
        {
            FrightenedOff();
            _frightenedTimer = 0f;
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

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩');
    }
}