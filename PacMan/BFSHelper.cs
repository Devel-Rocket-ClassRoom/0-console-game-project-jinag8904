
class BFSHelper
{
    private Tile[,] BFSMap;

    public BFSHelper()
    {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                char tile = MapManager.MapBase[y][x];

                BFSMap[y, x] = tile switch
                {
                    '#' => Tile.Wall,
                    _ => Tile.Empty
                };
            }
        }
    }

    public void BFS()
    {
        
    }
}