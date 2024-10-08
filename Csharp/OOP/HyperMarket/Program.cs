﻿namespace HyperMarket;

class Program
{
    static void Main(string[] args)
    {
        Shop shop = new Shop();

        shop.Work();
    }
}

public class Shop
{
    private Client _client = new();

    private int _profit;

    private readonly Queue<Client> _clients;

    private List<Item> _items = [];

    public Shop()
    {
        GenerateItems();

        _clients = FillClients();

        _profit = 0;
    }

    public void Work()
    {
        while (_clients.Count > 0)
        {
            _client = _clients.Dequeue();

            ShowClientInfo();

            while (CanClientPay() == false)
            {
                Console.WriteLine("Not enough money. I will remove random item from cart.");
                _client.RemoveItem();
            }

            SellItems();

            Console.WriteLine($"\nShop profit: {_profit}");

            Console.WriteLine("Press any key to serve another customer.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void SellItems()
    {
        _profit += _client.SumOfCart;
        GiveItemsToCustomer();
    }

    private void GiveItemsToCustomer()
    {
        _client.FillBag();
    }

    private void GenerateItems()
    {
        _items =
        [
            new Item("Apple", 10),
            new Item("Banana", 15),
            new Item("Orange", 20),
            new Item("Pineapple", 25),
            new Item("Watermelon", 30),
            new Item("Grape", 35),
            new Item("Kiwi", 40),
            new Item("Mango", 45),
            new Item("Peach", 50),
            new Item("Plum", 55),
            new Item("Cherry", 60),
            new Item("Strawberry", 65),
            new Item("Blueberry", 70),
            new Item("Avocado", 75),
            new Item("Lemon", 80)
        ];
    }

    private Client GenerateClient()
    {
        const int MaxCartCount = 5;
        const int MinCartCount = 1;
        int cartCount = ConsoleUtils.GetRandomnNumber(MinCartCount, MaxCartCount + 1);

        Client client = new Client();

        for (int i = 0; i < cartCount; i++)
        {
            client.InsertItemIntoCart(_items[ConsoleUtils.GetRandomnNumber(0, _items.Count - 1)]);
        }

        return client;
    }

    private Queue<Client> FillClients()
    {
        Queue<Client> clients = new Queue<Client>();

        int clientCount = 5;

        for (int i = 0; i < clientCount; i++)
        {
            clients.Enqueue(GenerateClient());
        }

        return clients;
    }

    private void ShowClientInfo()
    {
        _client.ShowCart();

        Console.WriteLine($"I have {_client.Money} money.");
    }

    private bool CanClientPay()
    {
        return _client.Money >= _client.SumOfCart;
    }
}

public class Client
{
    private int _minMoney = 200;
    private int _maxMoney = 500;

    private Inventory _cart;

    private Inventory _bag;

    public Client()
    {
        _bag = new Inventory();
        _cart = new Inventory();

        Money = ConsoleUtils.GetRandomnNumber(_minMoney, _maxMoney + 1);
    }

    public int SumOfCart => _cart.SumOfItems();

    public int Money { get; }

    public void InsertItemIntoCart(Item item)
    {
        _cart.AddItem(item);
    }

    public void ShowCart()
    {
        _cart.ShowItems();
    }

    public void FillBag()
    {
        foreach (Item item in _cart.Items)
        {
            _bag.AddItem(item);
        }

        _cart.Clear();
    }

    public void RemoveItem()
    {
        _cart.RemoveItem();
    }
}

public class Inventory
{
    private readonly List<Item> _items;

    public Inventory()
    {
        _items = new List<Item>();
    }

    public List<Item> Items => _items.ToList();

    public void AddItem(Item item)
    {
        _items.Add(item);
    }

    public void ShowItems()
    {
        foreach (Item item in _items)
        {
            item.ShowInfo();
        }

        Console.WriteLine("Total price: " + SumOfItems());
    }

    public int SumOfItems()
    {
        int totalCartSum = 0;

        foreach (Item item in _items)
        {
            totalCartSum += item.Price;
        }

        return totalCartSum;
    }

    public void RemoveItem()
    {
        _items.RemoveAt(ConsoleUtils.GetRandomnNumber(0, _items.Count - 1));
    }

    public void Clear()
    {
        _items.Clear();
    }
}

public class Item
{
    private string _name;

    public Item(string name, int price)
    {
        _name = name;
        Price = price;
    }

    public int Price { get; private set; }

    public void ShowInfo()
    {
        Console.WriteLine($"Name: {_name}, Price: {Price}");
    }
}

public class ConsoleUtils
{
    private static Random s_random = new Random();

    public static int GetRandomnNumber(int minNumber, int maxNumber)
    {
        int random = s_random.Next(minNumber, maxNumber + 1);

        return random;
    }
}