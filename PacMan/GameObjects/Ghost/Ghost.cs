using Framework.Engine;
using System;

abstract class Ghost : GameObject
{
    protected BFSHelper bfs = new();
    protected Random rand = new();

    public (int x, int y)[] directions = { (0, -1), (-1, 0), (0, 1), (1, 0) };

    public (int x, int y) Position;
    protected (int x, int y) homePos;
    protected (int x, int y) targetPos;

    protected const float k_MoveInterval = 0.15f;
    protected const float k_FrightenedMoveInterval = 0.3f;
    protected const float k_GoingHomeMoveInterval = 0.05f;
    protected float currentMoveInterval;

    protected const float k_FrightenedDuration = 10f;
    protected float waitingDuration = 0;

    protected (int x, int y) _nextDirection;
    
    protected float _moveTimer;
    protected float _frightenedTimer;
    protected float _waitingTimer;

    protected ConsoleColor basicColor;
    protected char basicPrint = '유';

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
        SetNextMove(PacMan.Position, PacMan.direction);

        _moveTimer += deltaTime;
        if (_waitingTimer < waitingDuration) _waitingTimer += deltaTime;    // 대기
        
        else
        {
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

        // frightened인 경우에는 랜덤으로 움직임
        else if (frightened)
        {
            // targetPos는 랜덤(...인데 개선 필요할듯)
            targetPos.x = rand.Next(0, 28);
            targetPos.y = rand.Next(0, 31);
        }

        // 적용
        _nextDirection = GetBestDir(targetPos);
    }

    protected (int x, int y) GetBestDir((int x, int y) targetPos)
    {
        int minDistance = int.MaxValue;
        (int dx, int dy) bestDir = (0, 0);

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
        // frightened 모드 끝나기 3초 전 깜빡깜빡 효과 필요

        var c = basicPrint;
        var color = basicColor;

        if (frightened)
        {
            c = '꺅';
            color = ConsoleColor.Blue;
        }

        if (goingHome)
        {
            c = '으';
            color = ConsoleColor.White;
        }

        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, c, color);
    }
}