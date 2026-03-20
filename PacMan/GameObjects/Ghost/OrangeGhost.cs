using Framework.Engine;
using System;

class OrangeGhost : Ghost
{
    public OrangeGhost(Scene scene) : base(scene)
    {
        Name = "Orange";
        Position = (15, 14);
        homePos = (15, 14);
        basicColor = ConsoleColor.DarkYellow;
        MapManager.MapTile[12, 14] |= Tile.OrangeGhost;

        // 대기 시간 초기화
        waitingDuration = 10f;
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        // 타겟 설정

        base.SetNextMove(pacManPos, pacManDir);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.OrangeGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.OrangeGhost;  // 새 위치에 플래그 설정
    }
}