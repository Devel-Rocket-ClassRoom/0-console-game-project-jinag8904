using Framework.Engine;
using System;

class PacMan : GameObject
{
    private const float k_MoveInterval = 0.15f; // 이동 텀

    private (int x, int y) _direction;
    private (int x, int y) _nextDirection;
    private float _moveTimer;

    public bool Alive { get; private set; } = true;
    public bool AtePowerPellet { get; set; } = false;

    public (int x, int y) Position;

    public PacMan(Scene scene) : base(scene)
    {
        Name = "PacMan";

        Position = (14, 23);
        MapManager.MapTile[23, 14] |= Tile.PacMan;

        _direction = (-1, 0);
        _nextDirection = (-1, 0);
    }

    public override void Update(float deltaTime)
    {
        if (!Alive) return;

        HandleInput();

        _moveTimer += deltaTime;

        if (_moveTimer > k_MoveInterval)
        {
            Move();
            _moveTimer = 0f;
        }
    }

    private void HandleInput()
    {
        (int x, int y) desired = _nextDirection;

        if (Input.IsKeyDown(ConsoleKey.UpArrow))
        {
            desired = (0, -1);
        }
        else if (Input.IsKeyDown(ConsoleKey.DownArrow))
        {
            desired = (0, 1);
        }
        else if (Input.IsKeyDown(ConsoleKey.LeftArrow))
        {
            desired = (-1, 0);
        }
        else if (Input.IsKeyDown(ConsoleKey.RightArrow))
        {
            desired = (1, 0);
        }

        _nextDirection = desired;
    }

    private void Move()
    {
        // !!!맵 양쪽 끝 처리 필요!!!

        var nextStep = (x: Position.x + _nextDirection.x, y: Position.y + _nextDirection.y);    // 다음 방향
        if (!MapManager.MapTile[nextStep.y, nextStep.x].HasFlag(Tile.Wall))
        {
            _direction = _nextDirection;
        }

        var finalPos = (x: Position.x + _direction.x, y: Position.y + _direction.y);    // 다음 위치

        if (!MapManager.MapTile[finalPos.y, finalPos.x].HasFlag(Tile.Wall))
        {
            MapManager.MapTile[Position.y, Position.x] &= ~Tile.PacMan; // 원래 위치에서 팩맨 플래그 제거
            Position = finalPos;                                        // 좌표 갱신
            MapManager.MapTile[Position.y, Position.x] |= Tile.PacMan;  // 새 위치에 팩맨 플래그 설정

            if (MapManager.MapTile[Position.y, Position.x].HasFlag(Tile.Ghost))
            {
                if (!AtePowerPellet) Alive = false;
            }

            else if (MapManager.MapTile[Position.y, Position.x].HasFlag(Tile.Pellet))
            {
                MapManager.MapTile[Position.y, Position.x] &= ~Tile.Pellet;
                GameScene.score += 10;
                GameScene.scoreText = GameScene.score.ToString();
            }

            else if (MapManager.MapTile[Position.y, Position.x].HasFlag(Tile.PowerPellet)) // 파워 펠렛
            {
                MapManager.MapTile[Position.y, Position.x] &= ~Tile.PowerPellet;
                GameScene.score += 50;
                GameScene.scoreText = GameScene.score.ToString();

                GameScene.fightenedModeTimer = 0;

                AtePowerPellet = true;
            }
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        var c = ' ';

        switch (_direction)
        {
            case (1, 0):
                c = '▶';
                break;
            case (-1, 0):
                c = '◀';
                break;
            case (0, 1):
                c = '▼';
                break;
            case (0, -1):
                c = '▲';
                break;
        }

        buffer.SetCell(Position.x +MapManager.Left, Position.y +MapManager.Top, c, ConsoleColor.Yellow);
    }
}