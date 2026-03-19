using Framework.Engine;
using System;

interface IGhost
{
    void Behave();

    void FrightenedBehave();
}

class Ghost : GameObject
{
    public (int x, int y) Position;
    public bool frightened;

    protected Ghost(Scene scene) : base(scene)
    {

    }

    public override void Update(float deltaTime)
    {

    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩');
    }
}

class RedGhost : Ghost, IGhost
{
    public RedGhost(Scene scene) : base(scene)
    {
        Name = "Red";
        frightened = false;

        Position = (14, 11);
        MapManager.MapTile[11, 14] |= Tile.Ghost;
    }

    public override void Update(float deltaTime)
    {

    }

    public void Behave()
    {

    }

    public void FrightenedBehave()
    {

    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.SetCell(Position.x + MapManager.Left, Position.y + MapManager.Top, '∩', ConsoleColor.Red);
    }
}

