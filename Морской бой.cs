public class Ship
{
    public List<(int x, int y)> Coordinates { get; private set; }
    public bool IsSunk => Coordinates.All(c => c == (0, 0)); // Упрощённая логика потопления

    public Ship(List<(int x, int y)> coordinates)
    {
        Coordinates = coordinates;
    }

    public void Hit(int x, int y)
    {
        // Если координаты попадания совпадают с координатами корабля, "потопить" часть корабля
        for (int i = 0; i < Coordinates.Count; i++)
        {
            if (Coordinates[i] == (x, y))
            {
                Coordinates[i] = (0, 0); // Координаты уничтожены
                break;
            }
        }
    }
}
public class Board
{
    private int size = 10; // Стандартный размер поля 10x10
    public List<Ship> Ships { get; private set; }

    public Board()
    {
        Ships = new List<Ship>();
    }

    public void AddShip(Ship ship)
    {
        Ships.Add(ship);
    }

    public bool Attack(int x, int y)
    {
        foreach (var ship in Ships)
        {
            ship.Hit(x, y);
            if (ship.IsSunk)
            {
                Console.WriteLine("Корабль потоплен!");
                return true;
            }
        }

        Console.WriteLine("Мимо!");
        return false;
    }
}
public class Player
{
    public string Name { get; set; }
    public Board Board { get; private set; }
    public bool IsAlive => Board.Ships.Any(s => !s.IsSunk); // Проверка на наличие живых кораблей

    public Player(string name)
    {
        Name = name;
        Board = new Board();
    }

    public void PlaceShip(Ship ship)
    {
        Board.AddShip(ship);
    }

    public bool Attack(Player opponent, int x, int y)
    {
        Console.WriteLine($"{Name} атакует координаты ({x}, {y})!");
        return opponent.Board.Attack(x, y);
    }
}
public class Game
{
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }
    private Player CurrentPlayer;

    public Game(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
        CurrentPlayer = player1;
    }

    public void SwitchTurn()
    {
        CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
    }

    public void Start()
    {
        Console.WriteLine("Игра началась!");

        while (Player1.IsAlive && Player2.IsAlive)
        {
            Console.WriteLine($"{CurrentPlayer.Name}, ваш ход!");

            // Получаем координаты для атаки от игрока (например, через консоль)
            Console.WriteLine("Введите координаты для атаки (x y): ");
            var input = Console.ReadLine()?.Split(' ');
            if (input != null && input.Length == 2 &&
                int.TryParse(input[0], out int x) && int.TryParse(input[1], out int y))
            {
                var hit = CurrentPlayer.Attack(CurrentPlayer == Player1 ? Player2 : Player1, x, y);
                if (!hit)
                {
                    SwitchTurn(); // Если мимо, то передаем ход
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод.");
            }
        }

        // Завершаем игру, когда один из игроков не имеет живых кораблей
        if (Player1.IsAlive)
            Console.WriteLine($"{Player1.Name} победил!");
        else
            Console.WriteLine($"{Player2.Name} победил!");
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        // Создание игроков
        var player1 = new Player("Игрок 1");
        var player2 = new Player("Игрок 2");

        // Размещение кораблей
        var ship1 = new Ship(new List<(int, int)> { (1, 1), (1, 2) });
        var ship2 = new Ship(new List<(int, int)> { (5, 5), (5, 6) });

        player1.PlaceShip(ship1);
        player2.PlaceShip(ship2);

        // Создание и запуск игры
        var game = new Game(player1, player2);
        game.Start();
    }
}
