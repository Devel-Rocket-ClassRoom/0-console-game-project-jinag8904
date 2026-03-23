
using System;
using System.Collections.Generic;

class BFSHelper
{
    private bool[,] visited = new bool[31, 28];   // 방문 체크
    private Queue<(int x, int y, int dist)> BFSqueue;   // 좌표 (x, y), 시작점으로부터의 거리

    private (int x, int y)[] directions = { (0, -1), (-1, 0), (0, 1), (1, 0) };

    public BFSHelper()
    {
        BFSqueue = new();
    }

    public int getDist((int x, int y) startPos, (int x, int y) targetPos)
    {
        BFSqueue.Clear();   // 초기화
        visited = new bool[31, 28];

        //1. 큐(Queue)에 시작 좌표 넣기
        BFSqueue.Enqueue((startPos.x, startPos.y, 0));
        visited[startPos.y, startPos.x] = true;

        //2. While(큐에 데이터가 있는 동안) 루프 돌리기
        while (BFSqueue.Count > 0)
        {
            var current = BFSqueue.Dequeue();

            if ((current.x, current.y) == (targetPos.x, targetPos.y)) // 큐에서 꺼낸 좌표가 타겟의 좌표와 같으면 거리 반환
            {
                return current.dist;
            }

            foreach (var dir in directions) // 3. 상하좌우 검사
            {
                var tPos = (x: current.x + dir.x, y: current.y + dir.y);    // 검사할 위치

                if (tPos.x < 0) tPos.x = 27;
                else if (tPos.x > 27) tPos.x = 0;

                if (tPos.x < 0 || 27 < tPos.x || tPos.y < 0 || 30 < tPos.y || // 범위 밖
                    visited[tPos.y, tPos.x] ||                                  // 방문한 타일
                    (MapManager.MapTile[tPos.y, tPos.x] & Tile.Wall) != 0)      // 벽
                {
                    continue;
                }

                // 4. 앞 조건에서 걸리지 않았으면 방문 표시, 큐에 값 삽입 (거리 1 증가)
                visited[tPos.y, tPos.x] = true;
                BFSqueue.Enqueue((tPos.x, tPos.y, current.dist +1));
            }
        }

        return 999;
    }
}