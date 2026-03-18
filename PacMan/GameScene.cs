using Framework.Engine;
using System;
using System.Collections.Generic;

class GameScene : Scene
{
    private int score;
    private bool isGameOver;

    MapManager map;

    public event GameAction PlayAgainRequested;

    public override void Load()
    {
        score = 0;
        isGameOver = false;

        map = new(this);
        AddGameObject(map);
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
    }

    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        buffer.WriteText(1, 0, $"Score: {score}", System.ConsoleColor.Gray);

        if (isGameOver)
        {
            buffer.WriteTextCentered(15, "GAME OVER", ConsoleColor.Red);
            buffer.WriteTextCentered(17, "Press ENTER to Retry", ConsoleColor.White);
        }
    }
}