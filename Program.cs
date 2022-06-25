using System.Runtime.InteropServices;

namespace snake
{
    static class Field
    {
        public static Snake Snake { get; set; }
        private static int gameSpeed = 1;
        public static int GameSpeed
        {
            get => gameSpeed;
            set => gameSpeed = value <= 0 ? 1 : value;
        }
        private static int size = 10;
        public static int Size
        {
            get => size;
            set => size = value < 5 ? 5 : value;
        }
        private static string RendImage = "";

        static public void Moving()
        {
            while (true)
            {
                if (Snake.Tail.Contains(Snake.HeadPos) || (Snake.HeadPos.X >= Size || Snake.HeadPos.X < 0 || Snake.HeadPos.Y >= Size || Snake.HeadPos.Y < 0))
                {
                    Snake.Tail.Clear();
                    Program.Start();
                }
                if (Snake.HeadPos.Equals(Snake.ApplePosition))
                {
                    Snake.Eat();
                }
                Snake.Tail.Enqueue(Snake.HeadPos);
                Snake.HeadPos += Snake.Direction;
                if (Snake.Tail.Count >= Snake.Lenght)
                {
                    Snake.Tail.Dequeue();
                }
                Thread.Sleep(1000 / GameSpeed);
            }
        }


        static public void Render()
        {
            while (true)
            {
                RendImage = "";
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Vector vect = new(j, i);
                        if (Snake.Tail.Contains(vect)) RendImage += "██";
                        else if (Snake.ApplePosition.Equals(vect)) RendImage += "▓▓";
                        else RendImage += "  ";
                    }
                    RendImage += "░░\n";
                }
                for (int i = 0; i < Size; i++) RendImage += "░░";
                Console.Clear();
                Console.WriteLine(RendImage);
                Thread.Sleep(20);
            }
        }
    }
    class Snake
    {
        public Queue<Vector> Tail = new();
        public Vector HeadPos { get; set; }
        public static Vector ApplePosition { get; set; } = Vector.Random(Field.Size, Field.Size);
        public Vector GetRandomPosition() {
            List<Vector> allCorrectPos = new();
            for (int y = 0; y < Field.Size; y++) {
                for (int x = 0; x < Field.Size; x++) {
                    if (!Tail.Contains<Vector>(new(x, y))) {
                        allCorrectPos.Add(new(x, y));
                    }
                }
            }
            return allCorrectPos[new Random().Next(allCorrectPos.Count)];
        }
        public void Eat()
        {
            ApplePosition = GetRandomPosition();
            Lenght++;
        }
        private int len;
        public int Lenght
        {
            get => len;
            set => len = value < 0 ? 0 : value;
        }

        private Vector direct;
        public Vector Direction
        {
            get => direct;
            set => direct = value.Normalized().Equals(-direct) ? direct : value.Normalized();
        }
    }

    internal class Program
    {
        private static void Optimization()
        { // Списал(((

            const int STD_OUTPUT_HANDLE = -11;

            [DllImport("kernel32.dll")]
            static extern IntPtr GetStdHandle(int handle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool SetConsoleDisplayMode(IntPtr ConsoleHandle, uint Flags, IntPtr NewScreenBufferDimensions);

            var hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
            SetConsoleDisplayMode(hConsole, 1, IntPtr.Zero);
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        }
        private static Thread rendthread = new(Field.Render);
        private static Thread inputThread = new(CheckInput);
        private static void CheckInput()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.W:
                            Field.Snake.Direction = -Vector.Up;
                            break;
                        case ConsoleKey.S:
                            Field.Snake.Direction = -Vector.Down;
                            break;
                        case ConsoleKey.D:
                            Field.Snake.Direction = Vector.Right;
                            break;
                        case ConsoleKey.A:
                            Field.Snake.Direction = Vector.Left;
                            break;
                        default:
                            break;
                    }

                    Thread.Sleep((int)(100/(Field.GameSpeed/10d)));
                }
            }

        }
        public static void Start()
        {
            Field.Snake = new Snake() { Lenght = 10, Direction = Vector.Right, HeadPos = new(Field.Size / 2, Field.Size / 2) };
            try
            {
                rendthread.Start();
                inputThread.Start();
            }
            catch { }
            Field.Moving();
        }
        static void Main()
        {
            Optimization();
            Console.WriteLine("Set field size. Standart = 20");
            try { Field.Size = int.Parse(Console.ReadLine() ?? "20"); }
            catch { Field.Size = 20; }
            Console.WriteLine("Set game speed. Standart = 5");
            try { Field.GameSpeed = int.Parse(Console.ReadLine() ?? "5"); }
            catch { Field.GameSpeed = 5; }
            Start();
        }
    }
}
