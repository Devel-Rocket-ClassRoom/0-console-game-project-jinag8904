using Framework.Engine;
using System;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

class RedGhost : Ghost
{
    public RedGhost(Scene scene) : base(scene)
    {
        Name = "Red";
        Position = (13, 11);
        homePos = (13, 14);
        basicColor = ConsoleColor.Red;
        MapManager.MapTile[11, 14] |= Tile.RedGhost;
    }

    public override void Update(float deltaTime)
    {
        SetNextMove(PacMan.Position, PacMan.direction);
        base.Update(deltaTime);
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        targetPos = pacManPos;  // 기본 설정
        base.SetNextMove(pacManPos, pacManDir);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.RedGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.RedGhost;  // 새 위치에 플래그 설정
    }
}