using Framework.Engine;
using System;
using System.Collections.Generic;

class GameScene : Scene
{
    public static int score;
    public static string scoreText = "00";
    public static int PelletCount;

    public static int ghostCapturedCount = 0;

    private const float k_FrightenedModeDuration = 10f;
    public static float fightenedModeTimer;

    private const float k_WaitingTime = 3f;
    private static float waitingTimer;
    public static bool isStarted;
    public static bool isRunning;

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
        PelletCount = 0;
        
        ghostCapturedCount = 0;

        fightenedModeTimer = 0;
        waitingTimer = 0;
        isStarted = false;
        isRunning = false;

        isGameOver = false;
        powerEventOn = false;

        map = new (this);
        AddGameObject(map);

        pacMan = new(this);
        AddGameObject(pacMan);
       
        redGhost = new RedGhost(this);
        AddGameObject(redGhost);

        pinkGhost = new PinkGhost(this);
        AddGameObject(pinkGhost);

        mintGhost = new MintGhost(this);
        AddGameObject(mintGhost);

        orangeGhost = new OrangeGhost(this);
        AddGameObject(orangeGhost);

        ghosts.Add(redGhost);
        ghosts.Add(pinkGhost);
        ghosts.Add(mintGhost);
        ghosts.Add(orangeGhost);
    }

    public override void Unload()
    {
        ClearGameObjects();
    }

    public override void Update(float deltaTime)
    {
        if (isStarted)
        {
            if (isGameOver || PelletCount == 244)
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

            // mintGhost는 redGhost의 위치를 전달받는다
            mintGhost.UpdateRedPos(redGhost.Position);
            UpdateGameObjects(deltaTime);

            if (!pacMan.Alive)
            {
                isGameOver = true;
                return;
            }

            if (isRunning)
            {
                Tile tile = MapManager.MapTile[PacMan.Position.y, PacMan.Position.x];

                if ((tile & (Tile.RedGhost | Tile.PinkGhost | Tile.OrangeGhost | Tile.MintGhost)) != 0)
                {
                    List<Ghost> hitGhosts = new();

                    if ((tile & Tile.RedGhost) != 0) hitGhosts.Add(ghosts[0]);
                    if ((tile & Tile.PinkGhost) != 0) hitGhosts.Add(ghosts[1]);
                    if ((tile & Tile.MintGhost) != 0) hitGhosts.Add(ghosts[2]);
                    if ((tile & Tile.OrangeGhost) != 0) hitGhosts.Add(ghosts[3]);

                    foreach (Ghost ghost in hitGhosts)
                    {
                        if (!ghost.frightened) isGameOver = true;
                        else if (!ghost.goingHome)
                        {
                            waitingTimer = 0;
                            isRunning = false;
                            ghostCapturedCount++;
                            ghost.GoingHomeOn();
                        }

                        switch (ghostCapturedCount)
                        {
                            case 1:
                                score += 200;
                                pacMan.scorePrint = "200";
                                break;
                            case 2:
                                score += 400;
                                pacMan.scorePrint = "400";
                                break;
                            case 3:
                                score += 800;
                                pacMan.scorePrint = "800";
                                break;
                            case 4:
                                score += 1600;
                                pacMan.scorePrint = "1600";
                                break;
                        }

                        scoreText = score.ToString();
                    }
                }
            }

            else if (waitingTimer < 2)
            {
                waitingTimer += deltaTime;
            }

            else
            {
                isRunning = true;
            }
        }

        else if (waitingTimer < k_WaitingTime)
        {
            waitingTimer += deltaTime;
        }

        else
        {
            isStarted = true;
            isRunning = true;
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

        else if (PelletCount == 244)
        {
            buffer.WriteTextCentered(15, "GAME CLEAR", ConsoleColor.Yellow);
            buffer.WriteTextCentered(17, "Press ENTER to Retry", ConsoleColor.White);
        }

        else if (!isStarted)
        {
            buffer.WriteTextCentered(20, "READY!", ConsoleColor.Yellow);
        }
    }
}