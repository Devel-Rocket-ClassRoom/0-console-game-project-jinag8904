using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

class GameScene : Scene
{
    public static int score;
    public static string scoreText = "00";
    private bool isGameOver;

    MapManager map;
    PacMan pacMan;

    public event GameAction PlayAgainRequested;

    public override void Load()
    {
        score = 0;
        isGameOver = false;

        map = new(this);
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

        // 사탕을 먹은 경우 점수+, 타일맵에서 사탕 없어짐
        //if (pacMan.Position == )

        // 파워사탕을 먹은 경우, 팩맨은 각성, 유령은 도망
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