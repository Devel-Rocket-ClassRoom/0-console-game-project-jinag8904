using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

class GameScene : Scene
{
    public static int score;
    public static string scoreText = "00";
    public static int ghostCapturedCount = 0;

    private const float k_FrightenedModeDuration = 10f;
    private float _fightenedModeTimer;

    private bool isGameOver;
    public bool powerEventOn = false;

    MapManager map;
    PacMan pacMan;

    public event GameAction PlayAgainRequested;

    public static event GameAction OnFrightenedMode;
    public static event GameAction OffFrightenedMode;

    public override void Load()
    {
        score = 0;
        scoreText = "00";
        ghostCapturedCount = 0;

        _fightenedModeTimer = 0;

        isGameOver = false;
        powerEventOn = false;

        map = new (this);
        AddGameObject(map);

        pacMan = new(this);
        AddGameObject(pacMan);
    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
        if (isGameOver)
        {
            if (Input.IsKeyDown(ConsoleKey.Enter)) PlayAgainRequested?.Invoke();
            return;
        }

        UpdateGameObjects(deltaTime);

        if (!pacMan.Alive)
        {
            isGameOver = true;
            return;
        }

        if (pacMan.AtePowerPellet)
        {
            pacMan.AtePowerPellet = false;
            powerEventOn = true;
            OnFrightenedMode?.Invoke();            
        }

        if (powerEventOn)
        {
            _fightenedModeTimer += deltaTime;

            if (_fightenedModeTimer > k_FrightenedModeDuration)
            {
                OffFrightenedMode?.Invoke();
                powerEventOn = false;
                _fightenedModeTimer = 0f;
                ghostCapturedCount = 0;
            }
        }

        if (MapManager.MapTile[pacMan.Position.y, pacMan.Position.x].HasFlag(Tile.Pellet))
        {
            MapManager.MapTile[pacMan.Position.y, pacMan.Position.x] &= ~Tile.Pellet;
            score += 10;
            scoreText = score.ToString();
        }

        else if (MapManager.MapTile[pacMan.Position.y, pacMan.Position.x].HasFlag(Tile.PowerPellet)) // 파워 펠렛
        {
            MapManager.MapTile[pacMan.Position.y, pacMan.Position.x] &= ~Tile.PowerPellet;
            score += 50;
            scoreText = score.ToString();

            _fightenedModeTimer = 0;
        }

        if (MapManager.MapTile[pacMan.Position.y, pacMan.Position.x].HasFlag(Tile.Ghost)) // 고스트
        {
            if (true) // 그 고스트가 frightened 모드일 때
            {
                ghostCapturedCount++;

                switch (ghostCapturedCount)
                {
                    case 1:
                        score += 200;
                        break;
                    case 2:
                        score += 400;
                        break;
                    case 3:
                        score += 800;
                        break;
                    case 4:
                        score += 1600;
                        break;
                }

                scoreText = score.ToString();
            }
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        buffer.WriteTextCentered(0, $"SCORE", ConsoleColor.Gray);
        buffer.WriteTextCentered(1, $"{scoreText}", ConsoleColor.Gray);

        if (isGameOver)
        {
            buffer.WriteTextCentered(15, "GAME OVER", ConsoleColor.Red);
            buffer.WriteTextCentered(17, "Press ENTER to Retry", ConsoleColor.White);
        }
    }
}