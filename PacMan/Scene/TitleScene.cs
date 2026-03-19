using System;
using Framework.Engine;

public class TitleScene : Scene
{
    public event GameAction StartRequested;

    public override void Load() { }
    public override void Unload() { }

    public override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Enter))
        {
            StartRequested?.Invoke();
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteTextCentered(14, "PUSH ENTER BUTTON", ConsoleColor.Yellow);
        buffer.WriteTextCentered(16, "1 PLAYER ONLY", ConsoleColor.Blue);
        buffer.WriteTextCentered(18, "ESC: QUIT");
    }
}