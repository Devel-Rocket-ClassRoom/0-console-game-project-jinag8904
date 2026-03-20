using Framework.Engine;
using System;

class PinkGhost : Ghost
{
    public PinkGhost(Scene scene) : base(scene)
    {
        Name = "Pink";
        Position = (14, 14);
        homePos = (14, 14);
        basicColor = ConsoleColor.Magenta;
        MapManager.MapTile[14, 14] |= Tile.PinkGhost;
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        int targetX = pacManPos.x + (pacManDir.x * 4);
        int targetY = pacManPos.y + (pacManDir.y * 4);

        targetPos.x = Math.Clamp(targetX, 1, 26);   // 보정
        targetPos.y = Math.Clamp(targetY, 1, 29);

        base.SetNextMove(pacManPos, pacManDir);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.PinkGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.PinkGhost;  // 새 위치에 플래그 설정
    }
}