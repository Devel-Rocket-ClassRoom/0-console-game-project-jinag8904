using System;
using Framework.Engine;

public class MapManager : GameObject
{
    public const int Left = 0;
    public const int Top = 3;
    public const int Right = 27;
    public const int Bottom = 33;

    private string[] MapBase =
    {
        "############################",
        "#............##............#",
        "#.####.#####.##.#####.####.#",
        "#O####.#####.##.#####.####O#",
        "#.####.#####.##.#####.####.#",
        "#..........................#",
        "#.####.##.########.##.####.#",
        "#.####.##.########.##.####.#",
        "#......##....##....##......#",
        "######.##### ## #####.######",
        "     #.##### ## #####.#     ",
        "     #.##          ##.#     ",
        "     #.## ###GG### ##.#     ",
        "######.## #      # ##.######",
        "      .   #      #   .      ",
        "######.## #      # ##.######",
        "     #.## ######## ##.#     ",
        "     #.##          ##.#     ",
        "     #.## ######## ##.#     ",
        "######.## ######## ##.######",
        "#............##............#",
        "#.####.#####.##.#####.####.#",
        "#.####.#####.##.#####.####.#",
        "#O..##.......  .......##..O#",
        "###.##.##.########.##.##.###",
        "###.##.##.########.##.##.###",
        "#......##....##....##......#",
        "#.##########.##.##########.#",
        "#.##########.##.##########.#",
        "#..........................#",
        "############################"
    };

    public static Tile[,] MapTile = new Tile[31, 28];

    public MapManager(Scene scene) : base(scene)
    {
        for (int y = 0; y < 31; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                char tile = MapBase[y][x];
                
                MapTile[y, x] = tile switch
                {
                    '#' => Tile.Wall,
                    '.' => Tile.Pellet,
                    'O' => Tile.PowerPellet,
                    'G' => Tile.GhostHouse,
                    _ => Tile.Empty
                };
            }
        }
    }

    public override void Draw(ScreenBuffer buffer)
    {
        for (int y = 0; y < MapTile.GetLength(0); y++)
        {
            for (int x = 0; x < MapTile.GetLength(1); x++)
            {
                string s = "";
                var color = ConsoleColor.White;

                var currentTile = MapTile[y, x];

                if (currentTile.HasFlag(Tile.PacMan))   continue;   // 팩맨, 유령은 팩맨 클래스에서 그리므로 패스
                if (currentTile.HasFlag(Tile.Ghost))    continue;
                
                if (currentTile.HasFlag(Tile.Wall))
                {
                    s = "■";
                    color = ConsoleColor.Blue;
                }
                else if (currentTile.HasFlag(Tile.Pellet))
                {
                    s = "ㆍ";
                }
                else if (currentTile.HasFlag(Tile.PowerPellet))
                {
                    s = "●";
                }
                else if (currentTile.HasFlag(Tile.GhostHouse))
                {
                    s = "〓";
                }
                else
                {
                    s = "ㅤ";
                }

                buffer.WriteText(x + Left, y + Top, s, color);
            }
        }
    }

    public override void Update(float deltaTime) { }
}

[Flags]
public enum Tile
{
    Empty       = 0,
    Wall        = 1 << 0,
    GhostHouse  = 1 << 1,
    Pellet      = 1 << 2,
    PowerPellet = 1 << 3,
    Ghost       = 1 << 4,
    PacMan      = 1 << 5
}