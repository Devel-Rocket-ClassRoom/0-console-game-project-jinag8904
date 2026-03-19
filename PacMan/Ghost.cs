using Framework.Engine;
using System;

class Ghost : GameObject
{
    public (int x, int y)[] directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };

    public (int x, int y) Position;

    private const float k_MoveInterval = 0.13f;
    private (int x, int y) _nextDirection;
    private float _moveTimer;

    public bool frightened;
    public bool goingHome;

    protected Ghost(Scene scene) : base(scene)
    {
        
    }

    public override void Update(float deltaTime)
    {
        _moveTimer += deltaTime;

        if (_moveTimer > k_MoveInterval)
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
    }

    public bool IsHit() => MapManager.MapTile[Position.y, Position.x] == Tile.PacMan;

    public void FrightenedOn() => frightened = true;
    public void FrightenedOff() => frightened = false;

    public override void Draw(ScreenBuffer buffer) => buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩');
    
}

class RedGhost : Ghost
{
    public static bool frightened;
    public static bool goingHome;

    private (int x, int y) _nextDirection;

    public RedGhost(Scene scene) : base(scene)
    {
        Name = "Red";
        frightened = false;
        goingHome = false;

        Position = (14, 11);
        MapManager.MapTile[11, 14] |= Tile.RedGhost;

        GameScene.OnFrightenedMode += FrightenedOn;
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        double minDistance = double.MaxValue;
        (int nextX, int nextY) bestMove = (Position.x, Position.y);

        foreach (var dir in directions)
        {
            int testX = Position.x + dir.x;
            int testY = Position.y + dir.y;

            // 1. 벽 체크 (벽이 아닐 때만 후보로 등록)
            if (!MapManager.MapTile[testY, testX].HasFlag(Tile.Wall))
            {
                // 2. 타겟(팩맨)까지의 거리 계산 (피타고라스 정리: a^2 + b^2)
                double dist = Math.Pow(pacManPos.x - testX, 2) + Math.Pow(pacManPos.y - testY, 2);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestMove = (testX, testY);
                }
            }
        }

        _nextDirection = bestMove;
    }

    public override void Move()
    {
        base.Move();
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.RedGhost; // 원래 위치에서 플래그 제거
        MapManager.MapTile[Position.y, Position.x] |= Tile.RedGhost;  // 새 위치에 플래그 설정
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩', ConsoleColor.Red);
    }
}

