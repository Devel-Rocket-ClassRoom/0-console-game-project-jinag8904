using Framework.Engine;
using System;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

class RedGhost : Ghost
{
    public RedGhost(Scene scene) : base(scene)
    {
        Name = "Red";
        Position = (14, 11);
        MapManager.MapTile[11, 14] |= Tile.RedGhost;
    }

    public override void Update(float deltaTime)
    {
        SetNextMove(PacMan.Position, PacMan.direction);
        base.Update(deltaTime);
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        var targetPos = (x: 0, y: 0);

        // goingHome인 경우에는 집으로 간다
        if (goingHome)
        {
            // 이미 집에 있으면 goingHome 해제, 속도 변경
            if (Position == (14, 14))
            {
                goingHome = false;
                frightened = false;
                _frightenedTimer = 0;
                currentMoveInterval = k_MoveInterval;
            }

            // 가는 도중이면... targetPos는 집 (14, 14)
            else
            {
                targetPos = (14, 14);
            }
        }

        // frightened인 경우에는 랜덤으로 움직임
        else if (frightened)
        {
            // targetPos는 랜덤 (인데 개선 필요할듯)
            targetPos.x = rand.Next(0, 28);
            targetPos.y = rand.Next(0, 31);
        }

        else
        {
            targetPos = pacManPos;  // 기본 타겟 (유령별로 다른 부분)
        }

        // 적용
        _nextDirection = GetBestDir(targetPos);
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
        _nextDirection = (-_nextDirection.x, -_nextDirection.y); // 가다가 반대로 한 번 꺾음
    }

    public override void FrightenedOff() => base.FrightenedOff();    
    public override void GoingHomeOn() => base.GoingHomeOn();
    
    public override void Draw(ScreenBuffer buffer)
    {
    /* 
        if (_frightenedTimer > 7)
        {
            switch (_color)
            {
                case ConsoleColor.Blue:
                    _color = ConsoleColor.White;
                    break;
                case ConsoleColor.White:
                    _color = ConsoleColor.Blue;
                    break;
            }
        }
    */

        char c = '유';
        var color = ConsoleColor.Red;

        if (frightened)
        {
            c = '튀';
            color = ConsoleColor.Blue;
        }

        if (goingHome)
        {
            c = '집';
            color = ConsoleColor.White;
        }

        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, c, color);
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
}

