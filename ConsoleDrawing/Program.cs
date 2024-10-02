﻿using System;
using System.IO;

namespace ConsoleDrawing
{
    class Program
    {
        static char[,] screen = new char[25, 80];
        static ConsoleColor[,] screenColors = new ConsoleColor[25, 80];
        static int cursorX = 0, cursorY = 0;
        static ConsoleColor currentColor = ConsoleColor.White;
        static string currentChar = "█";
        static ConsoleColor cursorColor = ConsoleColor.White;

        static void InitScreen()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    screen[y, x] = ' ';
                    screenColors[y, x] = ConsoleColor.Black;
                }
            }
        }

        static void DrawScreen()
        {
            for (int y = 0; y < 25; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < 80; x++)
                {
                    Console.ForegroundColor = screenColors[y, x];
                    Console.Write(screen[y, x]);
                }
            }
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = cursorColor;
            Console.Write(" ");
            Console.ResetColor();
            Console.SetCursorPosition(cursorX, cursorY);
            Console.CursorVisible = true;
        }

        static void DisplaySettings()
        {
            Console.SetCursorPosition(0, 25);
            Console.WriteLine("Color: " + currentColor.ToString());
            Console.WriteLine("Cursor: " + cursorX + ", " + cursorY);
            Console.WriteLine("Character: " + currentChar);
            Console.WriteLine("Cursor Color: " + cursorColor.ToString());
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
            int newX = cursorX + dx, newY = cursorY + dy;
            if (newX >= 0 && newX < 80 && newY >= 0 && newY < 25)
            {
                cursorX = newX;
                cursorY = newY;
            }
        }

        static void DrawChar(string c, ConsoleColor color)
        {
            for (int i = 0; i < c.Length; i++)
            {
                screen[cursorY, cursorX + i] = c[i];
                screenColors[cursorY, cursorX + i] = color;
            }
        }

        static void SetColor(ConsoleColor color)
        {
            currentColor = color;
            Console.ForegroundColor = color;
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Edit");
            Console.WriteLine("2. New Drawing");
            Console.Write("Choose an option: ");
        }

        static void LoadExistingDrawing()
        {
            string filePath = "existing_drawing.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                for (int y = 0; y < lines.Length; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        screen[y, x] = lines[y][x];
                    }
                }
                DrawScreen();
            }
        }

        static void CreateNewDrawing()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 80; x++)
                {
                    screen[y, x] = ' ';
                }
            }
            DrawScreen();
        }

        static void SaveDrawing()
        {
            string filePath = "existing_drawing.txt";
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
            File.WriteAllLines(filePath, lines);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                DisplayMenu();
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                ConsoleKey key = keyInfo.Key;

                switch (key)
                {
                    case ConsoleKey.D2:
                        Console.Clear();
                        CreateNewDrawing();
                        InitScreen();
                        while (true)
                        {
                            ConsoleKeyInfo originalKeyInfo = Console.ReadKey();
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
                                    SetColor(ConsoleColor.DarkBlue);
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
                                    break;
                            }
                            if (originalKey == ConsoleKey.Escape)
                            {
                                break;
                            }
                            DrawScreen();
                            DisplaySettings();
                        }
                        break;
                }
            }
        }
    }
}