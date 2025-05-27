using LibraryApp.Interfaces;

namespace LibraryApp.Services;

public class SplashScreenService : ISplashScreenService
{
    static Random rand = new Random();

    public void ShowGreetingsAnimation()
    {
        Console.CursorVisible = false;
        OpenBook();
        FlipPages();
        ShowSmokeEffect();
        ShowDemonWithFlash();
        Console.CursorVisible = true;
    }

    static void OpenBook()
    {
        string[] openFrames = new[]
        {
            "     _______\n" +
            "    /      /,\n" +
            "   /      //\n" +
            "  /______//\n" +
            " (______(/",

            "     _______\n" +
            "    /     /|\n" +
            "   /     / |\n" +
            "  /_____/  |\n" +
            " (_____(   |",

            "     _______\n" +
            "    |     | |\n" +
            "    |     | |\n" +
            "    |_____| |\n" +
            "    (_____(  )",

            "   ___   _______\n" +
            "  |       | |       |\n" +
            "  |       | |       |\n" +
            "  |       | |       |\n" +
            "  |_______| |_______|"
        };

        foreach (var frame in openFrames)
        {
            Console.Clear();
            Console.WriteLine(frame);
            Thread.Sleep(500);
        }
    }

    static void FlipPages()
    {
        string[] pages = new[]
        {
            "   ___   _______\n" +
            "  |       | |       |\n" +
            "  | PAGE  | |       |\n" +
            "  |   1   | |       |\n" +
            "  |_______| |_______|",

            "   ___   _______\n" +
            "  |       | |       |\n" +
            "  | PAGE  | |       |\n" +
            "  |   2   | |       |\n" +
            "  |_______| |_______|",

            "   ___   _______\n" +
            "  |       | |       |\n" +
            "  | PAGE  | |       |\n" +
            "  |   3   | |       |\n" +
            "  |_______| |_______|"
        };

        foreach (var page in pages)
        {
            Console.Clear();
            Console.WriteLine(page);
            Thread.Sleep(600);
        }
    }

    static void ShowSmokeEffect()
    {
        int width = Console.WindowWidth;
        int height = Console.WindowHeight;

        for (int i = 0; i < height / 2; i++)
        {
            Console.Clear();

            for (int y = height / 2 - i; y < height / 2 + i; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < width; x++)
                {
                    if (rand.NextDouble() < 0.05)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(GetSmokeChar());
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }

            Thread.Sleep(150);
        }

        Console.ResetColor();
    }

    static char GetSmokeChar()
    {
        char[] chars = { '~', '^', '.', '`' };
        return chars[rand.Next(chars.Length)];
    }

    static void ShowDemonWithFlash()
    {
        string[] demonLines = new[]
        {
        "              (    )              ",
        "             ((((()))             ",
        "             |o    o|             ",
        "             |  __  |             ",
        "             |\\__/|              ",
        "            /       \\            ",
        "           /|       |\\           ",
        "          /_|_______|_\\          ",
        "            |     |              ",
        "           /       \\             ",
        "          /_________\\            ",
        "         |    ||    |            ",
        "        (____/  \\____)           ",
        "        //          \\\\          ",
        "                             "
    };

        string[][] fireFramesLeft = new[]
        {
        new[] { " (   )", "(  ) )", " )(_)(" },
        new[] { " )   (", "( )  )", "(_ )(" },
        new[] { "( ) ( ", " )  ) ", "(_(_)" }
    };

        string[][] fireFramesRight = new[]
        {
        new[] { "(   ) ", "(  ( )", "(_) (" },
        new[] { "(   ) ", " ) ( (", ") (_) " },
        new[] { "( ) ) ", "  ( ( ", ") (_)" }
    };

        int width = Console.WindowWidth;
        int height = Console.WindowHeight;
        int demonWidth = demonLines[0].Length;
        int startX = (width - demonWidth) / 2;
        int startY = (height - demonLines.Length) / 2;

        for (int frame = 0; frame < 20; frame++)
        {
            Console.Clear();

            int fireIndex = frame % fireFramesLeft.Length;

            for (int i = 0; i < demonLines.Length; i++)
            {
                int lineY = startY + i;
                if (lineY < 0 || lineY >= height) continue;

                Console.SetCursorPosition(0, lineY);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(fireFramesLeft[fireIndex][i % fireFramesLeft[fireIndex].Length]);

                Console.SetCursorPosition(startX, lineY);
                Console.ForegroundColor = (frame % 2 == 0) ? ConsoleColor.Red : ConsoleColor.DarkRed;
                Console.Write(demonLines[i]);

                Console.SetCursorPosition(startX + demonWidth + 1, lineY);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(fireFramesRight[fireIndex][i % fireFramesRight[fireFramesRight.Length - 1].Length]);
            }

            // Centered warning text
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition((width - 28) / 2, startY + demonLines.Length + 1);
            if (frame % 2 == 0)
                Console.Write(">>>>> WELCOME TO THE LIBRARY APP! <<<<<");

            Thread.Sleep(200);
        }

        Console.ResetColor();
    }
}

