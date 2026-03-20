using Framework.Engine;
using System;

class OrangeGhost : Ghost
{
    private (int x, int y) orangeArea = (1, 29);

    public OrangeGhost(Scene scene) : base(scene)
    {
        Name = "Orange";
        Position = (15, 14);
        homePos = (15, 14);
        basicColor = ConsoleColor.DarkYellow;
        MapManager.MapTile[12, 14] |= Tile.OrangeGhost;

        // 대기 시간 초기화
        waitingDuration = 15f;
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정 (팩맨 위치 변할 때마다 재설정)
    {
        // 거리가 멀 때(8칸 이상): 빨간 유령(Blinky)처럼 팩맨의 현재 위치(pacManPos.x, pacManPos.y)를 직접 타겟으로 삼습니다. (추격 모드)
        // 거리가 가까울 때(8칸 미만): 팩맨을 무시하고 자신의 고유 구역인 왼쪽 아래 구석(1, 29)을 타겟으로 삼아 도망갑니다. (산책 / 도망 모드)
        double distanceSquared = Math.Pow(Position.x - pacManPos.x, 2) + Math.Pow(Position.y - pacManPos.y, 2);

        if (distanceSquared >= 64) targetPos = pacManPos;
        else targetPos = orangeArea;

        base.SetNextMove(pacManPos, pacManDir);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.OrangeGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.OrangeGhost;  // 새 위치에 플래그 설정
    }
}