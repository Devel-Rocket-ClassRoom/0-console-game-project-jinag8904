using System;
using Framework.Engine;

public class PacManGame : GameApp
{
    private readonly SceneManager<Scene> _scenes = new();

    public PacManGame() : base(34, 30) { }
    public PacManGame(int width, int height) : base(width, height) { }

    protected override void Initialize()
    {
        ChangeToTitle();
    }

    protected override void Update(float deltaTime)
    {
        if (Input.IsKeyDown(ConsoleKey.Escape))
        {
            Quit();
            return;
        }

        _scenes.CurrentScene?.Update(deltaTime);
    }

    protected override void Draw()
    {
        _scenes.CurrentScene?.Draw(Buffer);
    }

    private void ChangeToTitle()
    {
        var title = new TitleScene();
        //title.StartRequested += ChangeToPlay;
        _scenes.ChangeScene(title);
    }

/*    private void ChangeToPlay()
    {
        var play = new PlayScene();
        play.PlayAgainRequested += ChangeToTitle;
        _scenes.ChangeScene(play);
    }*/
}