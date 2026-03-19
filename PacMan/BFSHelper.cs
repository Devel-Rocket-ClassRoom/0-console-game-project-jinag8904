
using System;
using System.Collections.Generic;

class BFSHelper
{
    private int[,] BFSMap;
    private Queue<(int x, int y)> BFSQueue;
    // 방문한 노드

    private (int x, int y)[] directions = { (1, 0), (-1, 0), (0, 1), (0, -1) };

    public BFSHelper()
    {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                char tile = MapManager.MapBase[y][x];

                BFSMap[y, x] = tile switch
                {
                    '#' => 1,
                    _ => 0
                };
            }
        }
    }
    
    //1. a 노드(시작 노드)를 방문한다. (방문한 노드 체크)
    //- 큐에 방문된 노드를 삽입(enqueue)한다.
    //- 초기 상태의 큐에는 시작 노드만이 저장
    //    - 즉, a 노드의 이웃 노드를 모두 방문한 다음에 이웃의 이웃들을 방문한다.
    //2. 큐에서 꺼낸 노드과 인접한 노드들을 모두 차례로 방문한다.
    //- 큐에서 꺼낸 노드를 방문한다.
    //- 큐에서 커낸 노드과 인접한 노드들을 모두 방문한다.
    //    - 인접한 노드가 없다면 큐의 앞에서 노드를 꺼낸다(dequeue).
    //- 큐에 방문된 노드를 삽입(enqueue)한다.
    //3. 큐가 소진될 때까지 계속한다.
}