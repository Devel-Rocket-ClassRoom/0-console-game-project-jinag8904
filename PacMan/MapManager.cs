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
        "     #.## ######## ##.#     ",
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
                    '.' => Tile.Candy,
                    'O' => Tile.PowerCandy,
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

                if (currentTile.HasFlag(Tile.PacMan))
                {
                    continue;   // 팩맨은 팩맨 클래스에서 그리므로 패스
                }

                if (currentTile.HasFlag(Tile.Wall))
                {
                    s = "▣";
                    color = ConsoleColor.Blue;
                }
                else if (currentTile.HasFlag(Tile.PowerCandy))
                {
                    s = "◎";
                }
                else if (currentTile.HasFlag(Tile.Candy))
                {
                    s = "○";
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
    Empty = 0,
    Wall = 1,
    Candy = 1 << 1,
    PowerCandy = 1 << 2,
    Ghost = 1 << 3,
    PacMan = 1 << 4
}