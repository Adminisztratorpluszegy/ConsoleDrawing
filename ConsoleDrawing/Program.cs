class Program
{
    static char[,] screen = new char[25, 80];
    static ConsoleColor[,] screenColors = new ConsoleColor[25, 80];
    static char[,] previousScreen = new char[25, 80];
    static ConsoleColor[,] previousScreenColors = new ConsoleColor[25, 80];
    static int cursorX = 0, cursorY = 0;
    static ConsoleColor currentColor = ConsoleColor.White;
    static string currentChar = "█";
    static ConsoleColor cursorColor = ConsoleColor.White;
    static void Main(string[] args)
    {
        while (true)
        {
            DisplayMenu();
        }
    }
    static void DisplayMenu()
    {
        Console.Clear();
        ConsoleKeyInfo keyInfo;
        bool optionSelected = false;
        int selectedOption = 0;
        int menuWidth = 20;
        int menuHeight = 4;
        int menuX = (Console.WindowWidth - menuWidth) / 2;
        int menuY = (Console.WindowHeight - menuHeight) / 2;
        for (int y = menuY - 1; y <= menuY + menuHeight; y++)
        {
            Console.SetCursorPosition(menuX - 1, y);
            Console.Write("│");
            Console.SetCursorPosition(menuX + menuWidth, y);
            Console.Write("│");
        }
        Console.SetCursorPosition(menuX - 1, menuY - 1);
        Console.Write("┌");
        Console.SetCursorPosition(menuX + menuWidth, menuY - 1);
        Console.Write("┐");
        Console.SetCursorPosition(menuX - 1, menuY + menuHeight);
        Console.Write("└");
        Console.SetCursorPosition(menuX + menuWidth, menuY + menuHeight);
        Console.Write("┘");
        do
        {
            for (int i = 0; i < 4; i++)
            {
                Console.SetCursorPosition(menuX, menuY + i);
                if (i == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                switch (i)
                {
                    case 0:
                        Console.WriteLine("1. Új rajz");
                        break;
                    case 1:
                        Console.WriteLine("2. Rajz betöltése");
                        break;
                    case 2:
                        Console.WriteLine("3. Rajz törlése");
                        break;
                    case 3:
                        Console.WriteLine("4. Kilépés");
                        break;
                }
            }
            Console.ResetColor();
            keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOption > 0)
                    {
                        selectedOption--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOption < 3)
                    {
                        selectedOption++;
                    }
                    break;
                case ConsoleKey.Enter:
                    optionSelected = true;
                    break;
            }
        } while (!optionSelected);
        switch (selectedOption)
        {
            case 0:
                CreateNewDrawing();
                break;
            case 1:
                LoadExistingDrawing();
                break;
            case 2:
                DeleteDrawing();
                break;
            case 3:
                Environment.Exit(0);
                break;
        }
    }
    static void CreateNewDrawing()
    {
        Console.Clear();
        InitScreen();
        DrawScreen();
        EditDrawing();
    }
    static void LoadExistingDrawing()
    {
        Console.Clear();
        string[] drawingFiles = Directory.GetFiles(".", "*.txt");
        if (drawingFiles.Length == 0)
        {
            Console.WriteLine("Nem található ilyen.");
            Console.WriteLine("Nyomjon meg egy gombot a menübe lépéshez");
            Console.ReadKey(true);
            return;
        }
        int selectedOption = 0;
        bool optionSelected = false;
        bool exitMenu = false;
        do
        {
            Console.Clear();
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Válassz egy fájlt: ");
            int menuWidth = 40;
            int menuHeight = drawingFiles.Length + 2;
            int menuX = (Console.WindowWidth - menuWidth) / 2;
            int menuY = (Console.WindowHeight - menuHeight) / 2;
            for (int y = menuY - 1; y <= menuY + menuHeight; y++)
            {
                Console.SetCursorPosition(menuX - 1, y);
                Console.Write("│");
                Console.SetCursorPosition(menuX + menuWidth, y);
                Console.Write("│");
            }
            Console.SetCursorPosition(menuX - 1, menuY - 1);
            Console.Write("┌");
            Console.SetCursorPosition(menuX + menuWidth, menuY - 1);
            Console.Write("┐");
            Console.SetCursorPosition(menuX - 1, menuY + menuHeight);
            Console.Write("└");
            Console.SetCursorPosition(menuX + menuWidth, menuY + menuHeight);
            Console.Write("┘");
            for (int i = 0; i < drawingFiles.Length; i++)
            {
                Console.SetCursorPosition(menuX, menuY + i + 1);
                if (i == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
            Console.ResetColor();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOption > 0)
                    {
                        selectedOption--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOption < drawingFiles.Length - 1)
                    {
                        selectedOption++;
                    }
                    break;
                case ConsoleKey.Enter:
                    optionSelected = true;
                    break;
                case ConsoleKey.Escape:
                    exitMenu = true;
                    optionSelected = true;
                    break;
            }
        } while (!optionSelected && !exitMenu);
        if (exitMenu)
        {
            return;
        }
        string selectedDrawingFile = drawingFiles[selectedOption];
        string[] lines = File.ReadAllLines(selectedDrawingFile);
        InitScreen();
        for (int y = 0; y < Math.Min(lines.Length, 25); y++)
        {
            for (int x = 0; x < Math.Min(lines[y].Length, 80); x++)
            {
                screen[y, x] = lines[y][x];
                screenColors[y, x] = ConsoleColor.White;
            }
        }
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.CursorVisible = false;
        DrawScreen();
        EditDrawing();
    }
    static void DeleteDrawing()
    {
        string[] drawingFiles = Directory.GetFiles(".", "*.txt");
        if (drawingFiles.Length == 0)
        {
            Console.WriteLine("Nem található ilyen.");
            Console.WriteLine("Nyomjon meg egy gombot a menübe lépéshez");
            Console.ReadKey(true);
            return;
        }
        int selectedOption = 0;
        bool optionSelected = false;
        bool exitMenu = false;
        do
        {
            Console.Clear();
            Console.SetCursorPosition(10, 10);
            Console.WriteLine("Válassz egy fájlt: ");
            int menuWidth = 40;
            int menuHeight = drawingFiles.Length + 2;
            int menuX = (Console.WindowWidth - menuWidth) / 2;
            int menuY = (Console.WindowHeight - menuHeight) / 2;
            for (int y = menuY - 1; y <= menuY + menuHeight; y++)
            {
                Console.SetCursorPosition(menuX - 1, y);
                Console.Write("│");
                Console.SetCursorPosition(menuX + menuWidth, y);
                Console.Write("│");
            }
            Console.SetCursorPosition(menuX - 1, menuY - 1);
            Console.Write("┌");
            Console.SetCursorPosition(menuX + menuWidth, menuY - 1);
            Console.Write("┐");
            Console.SetCursorPosition(menuX - 1, menuY + menuHeight);
            Console.Write("└");
            Console.SetCursorPosition(menuX + menuWidth, menuY + menuHeight);
            Console.Write("┘");
            for (int i = 0; i < drawingFiles.Length; i++)
            {
                Console.SetCursorPosition(menuX, menuY + i + 1);
                if (i == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
            Console.ResetColor();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedOption > 0)
                    {
                        selectedOption--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedOption < drawingFiles.Length - 1)
                    {
                        selectedOption++;
                    }
                    break;
                case ConsoleKey.Enter:
                    optionSelected = true;
                    break;
                case ConsoleKey.Escape:
                    exitMenu = true;
                    optionSelected = true;
                    break;
            }
        } while (!optionSelected && !exitMenu);
        if (exitMenu)
        {
            return;
        }
        string selectedDrawingFile = drawingFiles[selectedOption];
        File.Delete(selectedDrawingFile);
        Console.WriteLine($"A fájl sikeresen törölve: {selectedDrawingFile}");
        Console.WriteLine("Nyomjon meg egy gombot a menübe lépéshez");
        Console.ReadKey(true);
    }
    static void InitScreen()
    {
        for (int y = 0; y < 25; y++)
        {
            for (int x = 0; x < 80; x++)
            {
                screen[y, x] = ' ';
                screenColors[y, x] = ConsoleColor.Black;
                previousScreen[y, x] = ' ';
                previousScreenColors[y, x] = ConsoleColor.Black;
            }
        }
    }
    static void DrawScreen()
    {
        DrawDrawingAreaBorder();
        for (int y = 0; y < 25; y++)
        {
            for (int x = 0; x < 80; x++)
            {
                if (screen[y, x] != previousScreen[y, x] || screenColors[y, x] != previousScreenColors[y, x])
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = screenColors[y, x];
                    Console.Write(screen[y, x]);
                    previousScreen[y, x] = screen[y, x];
                    previousScreenColors[y, x] = screenColors[y, x];
                }
            }
        }
        Console.SetCursorPosition(cursorX, cursorY);
        Console.BackgroundColor = cursorColor;
        Console.Write(" ");
        Console.ResetColor();
        Console.SetCursorPosition(cursorX, cursorY);
        Console.CursorVisible = false;
    }
    static void DrawDrawingAreaBorder()
    {
        int width = 80;
        int height = 25;
        for (int x = 0; x < width; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write("─");
            Console.SetCursorPosition(x, height - 1);
            Console.Write("─");
        }
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write("│");
            Console.SetCursorPosition(width - 1, y);
            Console.Write("│");
        }
        Console.SetCursorPosition(0, 0);
        Console.Write("┌");
        Console.SetCursorPosition(width - 1, 0);
        Console.Write("┐");
        Console.SetCursorPosition(0, height - 1);
        Console.Write("└");
        Console.SetCursorPosition(width - 1, height - 1);
        Console.Write("┘");
    }
    static void EditDrawing()
    {
        while (true)
        {
            DrawScreen();
            DisplaySettings();
            ConsoleKeyInfo originalKeyInfo = Console.ReadKey(true);
            ConsoleKey originalKey = originalKeyInfo.Key;
            switch (originalKey)
            {
                case ConsoleKey.Backspace:
                    Backspace();
                    break;
                case ConsoleKey.UpArrow:
                    MoveCursor(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveCursor(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    DrawChar(currentChar, currentColor);
                    break;
                case ConsoleKey.D0:
                    break;
                case ConsoleKey.D1:
                    SetColor(ConsoleColor.Red);
                    break;
                case ConsoleKey.D2:
                    SetColor(ConsoleColor.Green);
                    break;
                case ConsoleKey.D3:
                    SetColor(ConsoleColor.Yellow);
                    break;
                case ConsoleKey.D4:
                    SetColor(ConsoleColor.Blue);
                    break;
                case ConsoleKey.D5:
                    SetColor(ConsoleColor.Magenta);
                    break;
                case ConsoleKey.D6:
                    SetColor(ConsoleColor.Cyan);
                    break;
                case ConsoleKey.D7:
                    SetColor(ConsoleColor.DarkGreen);
                    break;
                case ConsoleKey.D8:
                    SetColor(ConsoleColor.DarkYellow);
                    break;
                case ConsoleKey.D9:
                    SetColor(ConsoleColor.DarkRed);
                    break;
                case ConsoleKey.NumPad1:
                    currentChar = "█";
                    break;
                case ConsoleKey.NumPad2:
                    currentChar = "▓";
                    break;
                case ConsoleKey.NumPad3:
                    currentChar = "▒";
                    break;
                case ConsoleKey.NumPad4:
                    currentChar = "░";
                    break;
                case ConsoleKey.Escape:
                    SaveDrawing();
                    return;
            }
        }
    }
    static void DisplaySettings()
    {
        Console.SetCursorPosition(0, 25);
        Console.WriteLine($"Color: {currentColor}");
        Console.WriteLine($"Cursor: {cursorX}, {cursorY}");
        Console.WriteLine($"Character: {currentChar}");
        Console.WriteLine($"Cursor Color: {cursorColor}");
    }
    static void Backspace()
    {
        if (cursorX > 0)
        {
            cursorX--;
            screen[cursorY, cursorX] = ' ';
            screenColors[cursorY, cursorX] = ConsoleColor.Black;
            DrawScreen();
        }
    }
    static void MoveCursor(int dx, int dy)
    {
        int newX = cursorX + dx;
        int newY = cursorY + dy;
        if (newX >= 0 && newX < 80 && newY >= 0 && newY < 25)
        {
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(screen[cursorY, cursorX]);
            Console.ResetColor();
            cursorX = newX;
            cursorY = newY;
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = cursorColor;
            Console.Write(" ");
            Console.ResetColor();
        }
    }
    static void DrawChar(string c, ConsoleColor color)
    {
        for (int i = 0; i < c.Length; i++)
        {
            if (cursorX + i < 80)
            {
                screen[cursorY, cursorX + i] = c[i];
                screenColors[cursorY, cursorX + i] = color;
            }
        }
        DrawScreen();
    }
    static void SetColor(ConsoleColor color)
    {
        currentColor = color;
    }
    static void SaveDrawing()
    {
        Console.WriteLine("Add meg a menteni kívánt fájl nevét:");
        string? fileName = Console.ReadLine();
        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("Érvénytelen fájlnév. A mentés megszakítva.");
            return;
        }
        string[] lines = new string[25];
        for (int y = 0; y < 25; y++)
        {
            string line = "";
            for (int x = 0; x < 80; x++)
            {
                line += screen[y, x];
            }
            lines[y] = line;
        }
        string filePath = $"{fileName}.txt";
        File.WriteAllLines(filePath, lines);
        Console.WriteLine($"A rajz sikeresen mentve lett: {filePath}");
    }
}