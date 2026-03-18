using System;
using Framework.Engine;

public enum Tile { Wall, Dot, PowerPellet, Empty, GhostGate }

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
        "#O..##................##..O#",
        "###.##.##.########.##.##.###",
        "###.##.##.########.##.##.###",
        "#......##....##....##......#",
        "#.##########.##.##########.#",
        "#.##########.##.##########.#",
        "#..........................#",
        "############################"
    };
    public Tile[,] MapTile = new Tile[31, 28];

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
                    '.' => Tile.Dot,
                    'O' => Tile.PowerPellet,
                    'G' => Tile.GhostGate,
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
                var color = new ConsoleColor();

                switch (MapTile[y, x])
                {
                    case Tile.Wall:
                        s = "▣";
                        color = ConsoleColor.Blue;
                        break;
                    case Tile.Dot:
                        s = "○";
                        color = ConsoleColor.White;
                        break;
                    case Tile.PowerPellet:
                        s = "◎";
                        color = ConsoleColor.White;
                        break;
                    case Tile.GhostGate:
                        s = "￣";
                        color = ConsoleColor.White;
                        break;
                    default:
                        s = "ㅤ";
                        break;
                }

                buffer.WriteText(x +Left, y +Top, s, color);
            }
        }
    }

    public override void Update(float deltaTime) { }
}