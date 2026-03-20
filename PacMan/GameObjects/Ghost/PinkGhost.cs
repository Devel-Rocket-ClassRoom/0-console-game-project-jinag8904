using Framework.Engine;

class PinkGhost : Ghost
{
    public PinkGhost(Scene scene) : base(scene)
    {
        Name = "Pink";
        Position = (14, 11);
        MapManager.MapTile[11, 14] |= Tile.RedGhost;
    }
}