static void LoadExistingDrawing()
{
    string[] drawingFiles = Directory.GetFiles(".", "*.txt");

    if (drawingFiles.Length == 0)
    {
        Console.WriteLine("Nem található ilyen.");
        return;
    }

    int selectedIndex = 0;
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Navigáljon a nyilakkal és nyomjon Entert a kiválasztáshoz:");
        for (int i = 0; i < drawingFiles.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.WriteLine($"> {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
            else
            {
                Console.WriteLine($"  {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
        }

        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (selectedIndex > 0) selectedIndex--;
                break;
            case ConsoleKey.DownArrow:
                if (selectedIndex < drawingFiles.Length - 1) selectedIndex++;
                break;
            case ConsoleKey.Enter:
                LoadDrawing(drawingFiles[selectedIndex]);
                return;
            case ConsoleKey.Escape:
                return; // Exit the menu
        }
    }
}

static void LoadDrawing(string filePath)
{
    string[] lines = File.ReadAllLines(filePath);

    InitScreen();

    for (int y = 0; y < Math.Min(lines.Length, 25); y++)
    {
        for (int x = 0; x < Math.Min(lines[y].Length, 80); x++)
        {
            screen[y, x] = lines[y][x];
            screenColors[y, x] = ConsoleColor.White;
        }
    }

    DrawScreen();
    EditDrawing();
}

static void DeleteDrawing()
{
    string[] drawingFiles = Directory.GetFiles(".", "*.txt");

    if (drawingFiles.Length == 0)
    {
        Console.WriteLine("Nem található ilyen.");
        return;
    }

    int selectedIndex = 0;
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Navigáljon a nyilakkal és nyomjon Entert a törléshez:");
        for (int i = 0; i < drawingFiles.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.WriteLine($"> {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
            else
            {
                Console.WriteLine($"  {Path.GetFileNameWithoutExtension(drawingFiles[i])}");
            }
        }

        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (selectedIndex > 0) selectedIndex--;
                break;
            case ConsoleKey.DownArrow:
                if (selectedIndex < drawingFiles.Length - 1) selectedIndex++;
                break;
            case ConsoleKey.Enter:
                File.Delete(drawingFiles[selectedIndex]);
                Console.WriteLine($"A fájl sikeresen törölve: {drawingFiles[selectedIndex]}");
                return;
            case ConsoleKey.Escape:
                return; // Exit the menu
        }
    }
}