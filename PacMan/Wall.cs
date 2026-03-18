using Framework.Engine;
using System;
using System.Collections.Generic;

public class Wall : GameObject
{
    readonly (int x, int y) Position;
    
    public Wall(Scene scene, (int x, int y) position) : base(scene)
    {
        Name = "Wall";
        Position = position;
    }

    public override void Draw(ScreenBuffer buffer)
    {
        buffer.WriteText(Position.x, Position.y, "□");
    }

    public override void Update(float deltaTime) { }
}