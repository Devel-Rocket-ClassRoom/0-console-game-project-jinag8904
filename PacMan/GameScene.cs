using Framework.Engine;
using System;
using System.Collections.Generic;

class GameScene : Scene
{
    public static int score;
    public static string scoreText = "00";
    public static int ghostCapturedCount = 0;

    private const float k_FrightenedModeDuration = 10f;
    public static float fightenedModeTimer;

    public static  bool isGameOver;
    public bool powerEventOn = false;

    MapManager map;
    PacMan pacMan;

    List<Ghost> ghosts = new();
    Ghost redGhost;
    Ghost pinkGhost;
    Ghost orangeGhost;
    Ghost mintGhost;

    public event GameAction PlayAgainRequested;

    public static event GameAction OnFrightenedMode;
    public static event GameAction OffFrightenedMode;

    public override void Load()
    {
        score = 0;
        scoreText = "00";
        ghostCapturedCount = 0;

        fightenedModeTimer = 0;

        isGameOver = false;
        powerEventOn = false;

        map = new (this);
        AddGameObject(map);

        pacMan = new(this);
        AddGameObject(pacMan);
       
        redGhost = new RedGhost(this);
        AddGameObject(redGhost);

        ghosts.Add(redGhost);
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

        if (pacMan.AtePowerPellet)
        {
            pacMan.AtePowerPellet = false;
            powerEventOn = true;
            OnFrightenedMode?.Invoke();
        }

        if (powerEventOn)
        {
            fightenedModeTimer += deltaTime;

            if (fightenedModeTimer > k_FrightenedModeDuration)
            {
                OffFrightenedMode?.Invoke();
                powerEventOn = false;
                ghostCapturedCount = 0;
            }
        }

        UpdateGameObjects(deltaTime);

        if (!pacMan.Alive)
        {
            isGameOver = true;
            return;
        }

        Tile tile = MapManager.MapTile[PacMan.Position.y, PacMan.Position.x];

        if ((tile & (Tile.RedGhost | Tile.PinkGhost | Tile.OrangeGhost | Tile.MintGhost)) != 0)
        {
            switch (tile)
            {
                case Tile.RedGhost:
                    if (ghosts[0].frightened && !ghosts[0].goingHome)
                    {
                        ghostCapturedCount++;
                        ghosts[0].GoingHomeOn();
                    }

                    else if (ghosts[0].goingHome)
                    {
                        // 아무것도 하지 않음
                    }

                    else
                    {
                        isGameOver = true;
                    }

                    break;

                default:
                    break;
            }

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