using Gallows;
using System;
using System.Drawing;

Game game;
Point[] pointsGallows = new Point[7];
Point[] pointsText = new Point[4];
const int maxAttempts = 6;

try
{
    game = new Game(maxAttempts);
    while (true)
    {
        game.NewWord();

        Console.WriteLine($"Угадай по буквам слово. На каждое слово {maxAttempts} попыток.");

        UpdateGallows(0);
        NewSecretWord();
        NewWritedChars();
        NewTextUserHelp();

        while (game.GameStatus == GameStatus.InProgress)
        {
            ClearPrevSymbol();

            if (!char.TryParse(Console.ReadLine().ToLower(), out char symbol))
                continue;

            SymbolCheck symbolCheck = game.CheckSymbolInWord(symbol);

            if (symbolCheck == SymbolCheck.NotSuitable)
                continue;

            if (symbolCheck == SymbolCheck.NotIncluded)
            {
                UpdateTextUserHelp();
                UpdateGallows(game.Attempts);
            }

            UpdateWritedChars();
            UpdateSecretWord();

            if (game.GameStatus == GameStatus.Win)
            {
                ShowGameOverMessage(isWin: true);
                break;
            }

            if (game.GameStatus == GameStatus.Lose)
            {
                ShowSecretWord();
                ShowGameOverMessage(isWin: false);
                break;
            }
        }

        Console.Clear();
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

void UpdateTextUserHelp()
{
    Console.SetCursorPosition(pointsText[3].X, pointsText[3].Y);
    Console.Write($"Осталось попыток {game.MaxAttempts - game.Attempts}");
}

void NewTextUserHelp()
{
    Console.Write("Вводи букву и жми Enter ");
    pointsText[3] = new Point(Console.CursorLeft, Console.CursorTop);
    Console.WriteLine($"Осталось попыток {game.MaxAttempts - game.Attempts}");
    pointsText[1] = new Point(Console.CursorLeft, Console.CursorTop);
}


void ShowGameOverMessage(bool isWin)
{
    Console.SetCursorPosition(pointsGallows[0].X, pointsText[0].Y + 1);

    if (isWin)
        Console.WriteLine("Молодец! :) Попробуй другое слово. Нажми Enter");
    else
        Console.WriteLine("Не получилось :( Попробуй другое слово. Нажми Enter");
    

    Console.ReadLine();
}

void UpdateGallows(int errorCount)
{
    switch (errorCount)
    {
        case 0:
            for(int i = 0; i < pointsGallows.Length; i++)
            {
                Console.WriteLine();
                pointsGallows[i] = new Point(Console.CursorLeft, Console.CursorTop);
            }
            break;
        case 1:
            Console.SetCursorPosition(pointsGallows[0].X, pointsGallows[0].Y);
            Console.Write("______");
            Console.SetCursorPosition(pointsGallows[1].X, pointsGallows[1].Y);
            Console.Write("|/   |");
            Console.SetCursorPosition(pointsGallows[2].X, pointsGallows[2].Y);
            Console.Write("|    o");
            for (int i = 3; i < 6; i++)
            {
                Console.SetCursorPosition(pointsGallows[i].X, pointsGallows[i].Y);
                Console.Write("|");
            }
            Console.SetCursorPosition(pointsGallows[6].X, pointsGallows[6].Y);
            Console.Write("|__________");
            break;
        case 2:
            Console.SetCursorPosition(pointsGallows[2].X, pointsGallows[2].Y);
            Console.Write("|    O");
            break;
        case 3:
            Console.SetCursorPosition(pointsGallows[3].X, pointsGallows[3].Y);
            Console.Write("|    |");
            break;
        case 4:
            Console.SetCursorPosition(pointsGallows[3].X, pointsGallows[3].Y);
            Console.Write("|   /|\\");
            break;
        case 5:
            Console.SetCursorPosition(pointsGallows[4].X, pointsGallows[4].Y);
            Console.Write("|    |");
            break;
        case 6:
            Console.SetCursorPosition(pointsGallows[5].X, pointsGallows[5].Y);
            Console.Write("|   / \\");
            break;

    }
}

void ClearPrevSymbol()
{
    Console.SetCursorPosition(pointsText[1].X, pointsText[1].Y);
    Console.Write(new String(' ', Console.LargestWindowWidth));
    Console.SetCursorPosition(pointsText[1].X, pointsText[1].Y);
}


void NewSecretWord() 
{
    Console.WriteLine();
    Console.WriteLine();
    pointsText[2] = new Point(Console.CursorLeft, Console.CursorTop);

    for (int i = 0; i < game.Word.Length; i++)
    {
        Console.Write("_ ");
    }
}

void UpdateSecretWord()
{
    Console.SetCursorPosition(pointsText[2].X, pointsText[2].Y);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(pointsText[2].X, pointsText[2].Y);

    for (int i = 0; i < game.OpenedChars.Length; i++)
    {
        if (game.OpenedChars[i])
            Console.Write($"{game.Word[i]} ");
        else
            Console.Write("_ ");
    }
}

void ShowSecretWord()
{
    Console.SetCursorPosition(pointsText[2].X, pointsText[2].Y);
    foreach (char c in game.Word)
    {
        Console.Write($"{c} ");
    }
}


void NewWritedChars() 
{
    Console.WriteLine();
    Console.WriteLine();
    pointsText[0] = new Point(Console.CursorLeft, Console.CursorTop);
    Console.WriteLine();
}

void UpdateWritedChars() 
{
    Console.SetCursorPosition(pointsText[0].X, pointsText[0].Y);
    Console.Write($"Уже введены буквы: {string.Join(' ', game.WritedChars)}");
}
