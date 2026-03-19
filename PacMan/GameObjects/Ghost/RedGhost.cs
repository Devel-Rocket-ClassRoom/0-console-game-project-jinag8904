using Framework.Engine;
using System;

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

            // 가는 도중이면... targetPos는 집
        }

        // frightened인 경우에는 팩맨으로부터 도망친다
        else if (frightened)
        {
            // targetPos는 팩맨의 반대 방향?
        }

        else
        {
            // 팩맨을 쫓는다
            targetPos = pacManPos;
        }

        double minDistance = double.MaxValue;
        (int nextX, int nextY) bestMove = (Position.x, Position.y);

        foreach (var dir in directions)
        {
            int testX = Position.x + dir.x;
            int testY = Position.y + dir.y;

            // 1. 벽 체크 (벽이 아닐 때만 후보로 등록)
            if (!MapManager.MapTile[testY, testX].HasFlag(Tile.Wall) && !MapManager.MapTile[testY, testX].HasFlag(Tile.GhostHouse))
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

    public override void FrightenedOn() => base.FrightenedOn();
    public override void FrightenedOff() => base.FrightenedOff();    
    public override void GoingHomeOn() => base.GoingHomeOn();
    
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
            c = '집';
            color = ConsoleColor.White;
        }

        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, c, color);
    }
}

