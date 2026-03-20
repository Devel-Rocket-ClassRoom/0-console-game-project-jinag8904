using Framework.Engine;
using System;

abstract class Ghost : GameObject
{
    protected BFSHelper bfs = new();
    protected Random rand = new();

    private (int x, int y)[] directions = { (0, -1), (-1, 0), (0, 1), (1, 0) };

    public (int x, int y) Position;
    protected (int x, int y) homePos;
    public (int x, int y) targetPos;

    protected const float k_MoveInterval = 0.13f;
    protected const float k_FrightenedMoveInterval = 0.25f;
    protected const float k_GoingHomeMoveInterval = 0.05f;
    public float currentMoveInterval;

    protected const float k_FrightenedDuration = 10f;
    protected float waitingDuration;

    public (int x, int y) _nextDirection;
    
    protected float _moveTimer;
    protected float _frightenedTimer;
    protected float _waitingTimer;

    protected ConsoleColor basicColor;
    protected char basicPrint = '유';

    public bool frightened;
    public bool goingHome;

    private const float k_BlinkingInterval = 0.2f;
    private float _blinkingTimer;
    private ConsoleColor frightenedColor;

    protected (int x, int y) frightenedDest;

    public bool justCaptured;

    protected Ghost(Scene scene) : base(scene)
    {
        frightened = false;
        goingHome = false;

        currentMoveInterval = k_MoveInterval;
        _moveTimer = currentMoveInterval;
        _blinkingTimer = 0;

        GameScene.OnFrightenedMode += FrightenedOn;
        GameScene.OffFrightenedMode += FrightenedOff;

        frightenedColor = ConsoleColor.Blue;

        while (true)
        {
            frightenedDest = (rand.Next(1, 26), rand.Next(1, 29));
            if ((MapManager.MapTile[frightenedDest.y, frightenedDest.x] & Tile.Wall | Tile.Empty) == 0) break;
        }

        justCaptured = false;
    }

    public override void Update(float deltaTime)
    {
        SetNextMove(PacMan.Position, PacMan.direction);

        if (GameScene.isRunning)
        {
            _moveTimer += deltaTime;
            _blinkingTimer += deltaTime;

            if (frightened) _frightenedTimer += deltaTime;

            if (_frightenedTimer > k_FrightenedDuration)
            {
                FrightenedOff();
                _frightenedTimer = 0f;
            }

            if (_blinkingTimer > k_BlinkingInterval)
            {
                _blinkingTimer = 0f;

                // frightened 모드 끝나기 3초 전 깜빡깜빡 효과
                if (frightened && _frightenedTimer > k_FrightenedDuration - 3)
                {
                    frightenedColor = frightenedColor == ConsoleColor.Blue ? ConsoleColor.White : ConsoleColor.Blue;
                }
            }

            if (_waitingTimer < waitingDuration) _waitingTimer += deltaTime;    // 대기
            else
            {
                if (_moveTimer > currentMoveInterval)
                {
                    Move();
                    _moveTimer = 0f;
                }
            }
        }
    }

    public virtual void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)    // 팩맨 위치가 변할 때마다 위치를 받아 경로 재계산 (가상함수)
    {
        if (goingHome)
        {
            if (Position == homePos)
            {
                goingHome = false;
                FrightenedOff();
                _frightenedTimer = 0f;
                currentMoveInterval = k_MoveInterval;
            }

            else
            {
                targetPos = homePos;
            }
        }

        // frightened인 경우에는 랜덤으로 움직임 (목적지 도착 시 변경)
        else if (frightened)
        {
            if (Position == frightenedDest)
            {
                while (true)
                {
                    frightenedDest = (rand.Next(1, 26), rand.Next(1, 29));
                    if ((MapManager.MapTile[frightenedDest.y, frightenedDest.x] & Tile.Wall | Tile.Empty) == 0) break;   // 목적지가 벽이 아닐 때 while문 break;
                }
            }

            targetPos = frightenedDest;
        }

        // 목적지 적용
        _nextDirection = GetBestDir(targetPos);
    }

    protected (int x, int y) GetBestDir((int x, int y) targetPos)
    {
        int minDistance = int.MaxValue;
        (int dx, int dy) bestDir = _nextDirection;  // 기본값은 현 방향

        foreach (var dir in directions)
        {
            var nextX = Position.x + dir.x;
            var nextY = Position.y + dir.y;

            if (nextX < 0 || nextX > 27 || nextY < 0 || nextY > 30 ||
            (MapManager.MapTile[nextY, nextX] & Tile.Wall) != 0) continue;

            int distance = bfs.getDist((nextX, nextY), targetPos);

            if (distance < minDistance)
            {
                minDistance = distance;
                bestDir = dir;
            }
        }

        return bestDir;
    }

    public virtual void Move()
    {
        Position = (x: Position.x + _nextDirection.x, y: Position.y + _nextDirection.y);
        if (IsHit() && !frightened) GameScene.isGameOver = true;
    }

    public bool IsHit() => MapManager.MapTile[Position.y, Position.x] == Tile.PacMan;

    public virtual void FrightenedOn()
    {
        _frightenedTimer = 0;
        _blinkingTimer = 0;

        frightened = true;
        currentMoveInterval = k_FrightenedMoveInterval;
        frightenedColor = ConsoleColor.Blue;
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
        var c = basicPrint;
        var color = basicColor;

        if (frightened)
        {
            c = '런';
            color = frightenedColor;
        }

        if (goingHome)
        {
            if (justCaptured)
            {
                return; // 방금 잡은 유령 자리에 점수가 출력되어야 하기 때문에 스킵
            }
            else
            {
                c = '망';
                color = ConsoleColor.White;
            }
        }

        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, c, color);
    }

    public virtual void UpdateRedPos((int x, int y) redPos) { }
}