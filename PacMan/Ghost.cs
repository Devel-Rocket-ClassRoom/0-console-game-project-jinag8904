using Framework.Engine;
using System;

class Ghost : GameObject
{
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
        SetNextMove(PacMan.Position, PacMan.direction);

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

class RedGhost : Ghost
{
    public RedGhost(Scene scene) : base(scene)
    {
        Name = "Red";
        frightened = false;
        goingHome = false;

        Position = (14, 11);
        MapManager.MapTile[11, 14] |= Tile.RedGhost;

        GameScene.OnFrightenedMode += FrightenedOn;
        GameScene.OffFrightenedMode += FrightenedOff;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void SetNextMove((int x, int y) targetPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        // frightened인 경우에는 팩맨으로부터 도망친다
        // goingHome인 경우에는 집으로 간다 (빠르게)
            // 집에 있으면 goingHome 해제, 속도 변경

        double minDistance = double.MaxValue;
        (int nextX, int nextY) bestMove = (Position.x, Position.y);

        foreach (var dir in directions)
        {
            int testX = Position.x + dir.x;
            int testY = Position.y + dir.y;

            // 1. 벽 체크 (벽이 아닐 때만 후보로 등록)
            if (!(MapManager.MapTile[testY, testX].HasFlag(Tile.Wall)) && !(MapManager.MapTile[testY, testX].HasFlag(Tile.GhostHouse)))
            {
                // 2. 타겟(팩맨)까지의 거리 계산 (피타고라스 정리: a^2 + b^2)
                double dist = Math.Pow(targetPos.x - testX, 2) + Math.Pow(targetPos.y - testY, 2);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestMove = (testX, testY);
                }
            }
        }

        _nextDirection = (bestMove.nextX - Position.x, bestMove.nextY - Position.y);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.RedGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.RedGhost;  // 새 위치에 플래그 설정
    }

    public override void FrightenedOn()
    {
        base.FrightenedOn();
    }

    public override void FrightenedOff()
    {
        base.FrightenedOff();
    }

    public override void GoingHomeOn()
    {
        base.GoingHomeOn();
    }

    public override void Draw(ScreenBuffer buffer)
    {
        // '∩'
        char c = '유';
        var color = ConsoleColor.Red;

        if (frightened)
        {
            c = '튀';
            color = ConsoleColor.Blue;
        }

        if (goingHome)
        {
            c = 'ㅎ';
            color = ConsoleColor.White;
        }

        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, c, color);
    }
}

