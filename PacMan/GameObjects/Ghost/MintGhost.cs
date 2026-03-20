using Framework.Engine;
using System;

class MintGhost : Ghost
{
    public (int x, int y) redPos;

    public MintGhost(Scene scene) : base(scene)
    {
        Name = "Mint";
        Position = (12, 14);
        homePos = (12, 14);
        basicColor = ConsoleColor.Cyan;
        MapManager.MapTile[12, 14] |= Tile.MintGhost;

        // 대기 시간 초기화
        waitingDuration = 5f;
    }

    public override void SetNextMove((int x, int y) pacManPos, (int x, int y) pacManDir)   // 경로 탐색, 방향 설정
    {
        int gijunX = pacManPos.x + (pacManDir.x * 2);
        int gijunY = pacManPos.y + (pacManDir.y * 2);

        int diffX = gijunX - redPos.x;
        int diffY = gijunY - redPos.y;

        int targetX = redPos.x + (diffX * 2);
        int targetY = redPos.y + (diffY * 2);

        targetPos.x = Math.Clamp(targetX, 0, 27);   // 보정
        targetPos.y = Math.Clamp(targetY, 0, 30);

        base.SetNextMove(pacManPos, pacManDir);
    }

    public override void Move()
    {
        MapManager.MapTile[Position.y, Position.x] &= ~Tile.MintGhost; // 원래 위치에서 플래그 제거
        base.Move();
        MapManager.MapTile[Position.y, Position.x] |= Tile.MintGhost;  // 새 위치에 플래그 설정
    }

    public override void UpdateRedPos((int x, int y) redPos) => this.redPos = redPos;
}