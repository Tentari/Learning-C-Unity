﻿namespace Glad_Arena;

class Program
{
    static void Main(string[] args)
    {
        Arena arena = new Arena();

        arena.Work();

        Console.ReadKey();
    }
}

public class Arena
{
    private List<Gladiator> _allGladiators;

    private Gladiator _firstGladiator;
    private Gladiator _secondGladiator;

    public Arena()
    {
        _allGladiators = new List<Gladiator> { new Fighter(), new Tyyr(), new Tank(), new Mage(), new Dagger() };
        ChooseGladiators();
    }

    public void Work()
    {
        Console.WriteLine("Hello player. Choose your gladiators!");

        Console.Clear();
        Fight();

        WinnerMessage();
    }

    private void Fight()
    {
        int firstGladiatorHealth = _firstGladiator.Health;
        int secondGladiatorDamage;

        while (_firstGladiator.Health > 0 && _secondGladiator.Health > 0)
        {
            _firstGladiator.Attack(_secondGladiator);


            _secondGladiator.Attack(_firstGladiator);

            Console.WriteLine(
                $"First gladiator({_firstGladiator.Name}) HP:{_firstGladiator.Health} Second gladiator({_secondGladiator.Name}) HP:{_secondGladiator.Health}\n");
        }
    }

    private void WinnerMessage()
    {
        if (_firstGladiator.Health <= 0 && _secondGladiator.Health <= 0)
        {
            Console.WriteLine("Draw");
        }
        else if (_firstGladiator.Health > 0)
        {
            Console.WriteLine($"first gladiator({_firstGladiator.Name}) wins");
        }
        else if (_secondGladiator.Health > 0)
        {
            Console.WriteLine($"second gladiator({_secondGladiator.Name}) wins");
        }
    }

    private void ChooseGladiators()
    {
        ShowGladiators();

        Console.WriteLine("Choose first gladiator: ");
        _firstGladiator = CreateGladiator();

        ShowGladiators();

        Console.WriteLine("Choose second gladiator: ");
        _secondGladiator = CreateGladiator();
    }

    private Gladiator CreateGladiator()
    {
        Gladiator gladiator = null;

        bool isCreated = false;

        int index = ConsoleUtils.ReadInt() - 1;

        while (isCreated == false)
        {
            if (index < _allGladiators.Count && index >= 0)
            {
                isCreated = true;
                gladiator = _allGladiators[index].Clone();
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");

                index = ConsoleUtils.ReadInt() - 1;
            }
        }

        return gladiator;
    }

    private void ShowGladiators()
    {
        int gladiatorsNumber = 1;

        Console.WriteLine();

        foreach (Gladiator gladiator in _allGladiators)
        {
            Console.Write(gladiatorsNumber++ + ". ");
            gladiator.ShowInfo();
        }

        Console.WriteLine();
    }
}

public abstract class Gladiator
{
    public Gladiator(int health, int damage, string name)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public string Name { get; private set; }
    public int Health { get; protected set; }
    public int Damage { get; protected set; }

    public void ShowInfo()
    {
        Console.WriteLine($"{Name} - {Health} HP / {Damage} DMG");
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            Health -= damage;
        }
    }

    public virtual void Attack(Gladiator gladiator)
    {
        gladiator.TakeDamage(Damage);

        DrawDamageDone(gladiator);
    }

    public abstract Gladiator Clone();

    public void DrawDamageDone(Gladiator gladiator)
    {
        Console.WriteLine($"{Name} attacks {gladiator.Name} for {Damage}");
    }
}

public class Fighter : Gladiator
{
    private static Random s_random = new Random();

    public Fighter(int health = 1000, int damage = 50, string name = "Fighter") : base(health, damage, name)
    {
    }

    public override void Attack(Gladiator gladiator)
    {
        Gladiator enemy = gladiator;

        int minCritChance = 30;
        int tempDamage = Damage;

        if (ConsoleUtils.RoolChance(minCritChance))
        {
            Damage *= 2;

            Console.WriteLine("Critical hit!");

            base.Attack(enemy);

            Damage = tempDamage;
        }
        else
        {
            base.Attack(enemy);
        }
    }

    public override Gladiator Clone()
    {
        return new Fighter();
    }
}

public class Tyyr : Gladiator
{
    private int _attackCount;
    private int _maxAttackCount;

    public Tyyr(int health = 500, int damage = 200, string name = "Tyyr") : base(health, damage, name)
    {
        _attackCount = 1;
        _maxAttackCount = 3;
    }

    public override void Attack(Gladiator gladiator)
    {
        Gladiator enemy = gladiator;

        if (_attackCount == _maxAttackCount)
        {
            base.Attack(enemy);

            Console.WriteLine($"Double attack!");

            base.Attack(gladiator);

            _attackCount = 0;
        }
        else
        {
            base.Attack(gladiator);

            _attackCount++;
        }
    }

    public override Gladiator Clone()
    {
        return new Tyyr();
    }
}

public class Tank : Gladiator
{
    private int _maxHealth;
    private int _rage;

    public Tank(int health = 3000, int damage = 10, string name = "Tank") : base(health, damage, name)
    {
        _rage = 0;
        _maxHealth = health;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        int maxRage = 100;
        int rageGain = 20;

        if (_rage >= maxRage)
        {
            CastHeal();
            _rage = 0;
        }
        else
        {
            _rage += rageGain;
        }
    }

    public void CastHeal()
    {
        int healAmmount = 500;

        Health = Math.Min(_maxHealth, Health += healAmmount);

        if (Health == _maxHealth)
        {
            Console.WriteLine("Healed to full.");
        }
        else
        {
            Console.WriteLine($"Blessed heal! + {healAmmount} HP");
        }
    }

    public override Gladiator Clone()
    {
        return new Tank();
    }
}

public class Mage : Gladiator
{
    private int _mana;

    public Mage(int health = 1000, int damage = 5, string name = "Mage") : base(health, damage, name)
    {
        _mana = 100;
    }

    public override void Attack(Gladiator gladiator)
    {
        Gladiator enemy = gladiator;

        int tempDamage = Damage;

        if (_mana >= 20)
        {
            _mana -= 20;

            CastFireball();

            base.Attack(enemy);

            Damage = tempDamage;
        }
        else
        {
            base.Attack(gladiator);

            Console.WriteLine("No mana, attacking with hand.");
        }
    }

    public void CastFireball()
    {
        Console.WriteLine("Fireball casted!");

        Damage += 300;
    }

    public override Gladiator Clone()
    {
        return new Mage();
    }
}

public class Dagger : Gladiator
{
    public Dagger(int health = 1000, int damage = 100, string name = "Dagger") : base(health, damage, name)
    {
    }

    public override void TakeDamage(int damage)
    {
        int minNumberToDodge = 30;

        if (ConsoleUtils.RoolChance(minNumberToDodge))
        {
            Console.WriteLine("Dodged!");
            base.TakeDamage(0);
        }
        else
        {
            base.TakeDamage(damage);
        }
    }

    public override Gladiator Clone()
    {
        return new Dagger();
    }
}

public class ConsoleUtils
{
    private static Random s_random = new Random();

    public static bool RoolChance(int percent)
    {
        int random = s_random.Next(100 + 1);

        return random <= percent;
    }

    public static int ReadInt()
    {
        int number;

        while (int.TryParse(Console.ReadLine(), out number) == false)
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }

        return number;
    }
}