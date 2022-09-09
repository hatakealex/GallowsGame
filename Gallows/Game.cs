namespace Gallows
{
    public class Game
    {
        const string correctSymbols = "йцукенгшщзхъфывапролджэячсмитьбю";
        private readonly string[] words;

        public List<char> WritedChars { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public int MaxAttempts { get; private set; }
        public int Attempts { get; private set; }
        public string Word { get; private set; }
        public bool[] OpenedChars { get; private set; }



        public Game(int maxAttempts=6) 
        {
            if (maxAttempts < 5 || maxAttempts > 10)
                throw new ArgumentException("Количество попыток должно быть от 5 до 10");

            GameStatus = GameStatus.NotStarted;
            MaxAttempts = maxAttempts;
            words = File.ReadAllLines("WordsStockRus.txt");
        }

        public string NewWord() 
        {
            if (GameStatus == GameStatus.InProgress)
                throw new InvalidOperationException("Игра уже запущена");

            GameStatus = GameStatus.InProgress;
            Word = words[new Random(DateTime.Now.Millisecond).Next(0, words.Length)];
            OpenedChars = new bool[Word.Length];
            WritedChars = new List<char>();
            Attempts = 0;

            return Word;
        }

        public SymbolCheck CheckSymbolInWord(char symbol) 
        {
            int index = Word.IndexOf(symbol);
            bool check = correctSymbols.IndexOf(symbol) != -1 && WritedChars.IndexOf(symbol) == -1;
            if (!check)
                return SymbolCheck.NotSuitable;

            WritedChars.Add(symbol);
            WritedChars.Sort();

            if (index != -1)
            {
                for (int i = 0; i < Word.Length; i++)
                {
                    if (Word[i] == symbol)
                    {
                        OpenedChars[i] = true;
                    }
                }

                if (CheckWordOpened())
                    GameStatus = GameStatus.Win;

                return SymbolCheck.Included;
            }
            else
            {
                Attempts++;

                if(Attempts == MaxAttempts)
                    GameStatus = GameStatus.Lose;

                return SymbolCheck.NotIncluded;
            }
        }

        private bool CheckWordOpened()
        {
            foreach (bool opened in OpenedChars)
            {
                if (!opened)
                    return false;
            }

            return true;
        }
    }
}
