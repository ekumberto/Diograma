using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ConsoleApp2
{

    internal class Program
    {
        public class Item
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public double Value { get; set; } // Используем double вместо decimal
            public int Weight { get; set; }  // Используем int вместо float

            public Item() { }

            public Item(string name, string type, double value, int weight)
            {
                Name = name;
                Type = type;
                Value = value;
                Weight = weight;
            }

            public override string ToString()
            {
                return $"{Name} (Тип: {Type}, Стоимость: {Value}, Вес: {Weight})";
            }
        }
        public class Character
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Dexterity { get; set; }
            public int Intelligence { get; set; }
            public List<Item> Inventory { get; set; }

            public Character()
            {
                Inventory = new List<Item>();
            }

            public Character(string name, int level, int health, int strength, int dexterity, int intelligence)
            {
                this.Name = name;
                this.Level = level;
                this.Health = health;
                this.Strength = strength;
                this.Dexterity = dexterity;
                this.Intelligence = intelligence;
                this.Inventory = new List<Item>();
            }

            public override string ToString()
            {
                return Name + "Уровень: " + Level + "Здоровье: " + Health + "Сила: " + Strength + "Ловкость: " + Dexterity + "Интеллект: " + Intelligence;
            }
        }

        // Класс для игры
        public class Game
        {
            public List<Character> Characters { get; set; }
            public List<Item> Items { get; set; }

            public Game()
            {
                Characters = new List<Character>();
                Items = new List<Item>();
            }

            public void AddCharacter(Character character)
            {
                Characters.Add(character);
            }

            public bool RemoveCharacter(string name)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].Name == name)
                    {
                        Characters.RemoveAt(i);
                        return true;
                    }
                }
                return false;
            }

            public void AddItemToInventory(string name, Item item)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].Name == name)
                    {
                        Characters[i].Inventory.Add(item);
                    }
                }
            }

            public bool RemoveItemFromInventory(string name, string itemName)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].Name == name)
                    {
                        for (int j = 0; j < Characters[i].Inventory.Count; j++)
                        {
                            if (Characters[i].Inventory[j].Name == itemName)
                            {
                                Characters[i].Inventory.RemoveAt(j);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            public void ChangeCharacterLevel(string name, int newLevel)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].Name == name)
                    {
                        Characters[i].Level = newLevel;
                    }
                }
            }

            public void ChangeCharacterStats(string name, int newHealth, int newStrength, int newDexterity, int newIntelligence)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].Name == name)
                    {
                        Characters[i].Health = newHealth;
                        Characters[i].Strength = newStrength;
                        Characters[i].Dexterity = newDexterity;
                        Characters[i].Intelligence = newIntelligence;
                    }
                }
            }

            public void SerializeToJson(string filename)
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filename, json);
            }

            public static Game DeserializeFromJson(string filename)
            {
                string json = File.ReadAllText(filename);
                Game deserializedGame = JsonConvert.DeserializeObject<Game>(json);
                return deserializedGame;
            }

            public void SerializeToXml(string filename)
            {
                var xmlSerializer = new XmlSerializer(typeof(Game));
                using (var writer = new StreamWriter(filename))
                {
                    xmlSerializer.Serialize(writer, this);
                }
            }

            public static Game DeserializeFromXml(string filename)
            {
                var xmlSerializer = new XmlSerializer(typeof(Game));
                using (var reader = new StreamReader(filename))
                {
                    Game deserializedGame = (Game)xmlSerializer.Deserialize(reader);
                    return deserializedGame;
                }
            }
        }
        public class Program
        {
            public static void Main(string[] args)
            {
                Game game = new Game();

                bool running = true;
                while (running)
                {
                    Console.WriteLine("1. Добавить персонажа");
                    Console.WriteLine("2. Удалить персонажа");
                    Console.WriteLine("3. Добавить предмет в инвентарь персонажа");
                    Console.WriteLine("4. Удалить предмет из инвентаря персонажа");
                    Console.WriteLine("5. Изменить уровень персонажа");
                    Console.WriteLine("6. Показать инвентарь персонажа");
                    Console.WriteLine("7. Сохранить игру в JSON");
                    Console.WriteLine("8. Загрузить игру из JSON");
                    Console.WriteLine("9. Сохранить игру в XML");
                    Console.WriteLine("10. Загрузить игру из XML");
                    Console.WriteLine("11. Выход");

                    Console.Write("Выберите опцию: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddCharacter(game);
                            break;
                        case "2":
                            RemoveCharacter(game);
                            break;
                        case "3":
                            AddItemToCharacter(game);
                            break;
                        case "4":
                            RemoveItemFromCharacter(game);
                            break;
                        case "5":
                            ChangeCharacterLevel(game);
                            break;
                        case "6":
                            DisplayCharacterInventory(game);
                            break;
                        case "7":
                            SaveGameToJson(game);
                            break;
                        case "8":
                            LoadGameFromJson(ref game);
                            break;
                        case "9":
                            SaveGameToXml(game);
                            break;
                        case "10":
                            LoadGameFromXml(game);
                            break;
                        case "11":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Неверная опция. Попробуйте снова.");
                            break;
                    }
                }
            }

            private static void AddCharacter(Game game)
            {
                Console.Write("Введите имя персонажа: ");
                string name = Console.ReadLine();

                Console.Write("Введите уровень персонажа: ");
                int level = int.Parse(Console.ReadLine());

                Console.Write("Введите здоровье персонажа: ");
                int health = int.Parse(Console.ReadLine());

                Console.Write("Введите силу персонажа: ");
                int strength = int.Parse(Console.ReadLine());

                Console.Write("Введите ловкость персонажа: ");
                int dexterity = int.Parse(Console.ReadLine());

                Console.Write("Введите интеллект персонажа: ");
                int intelligence = int.parse(Console.ReadLine());

                Character character = new Character(name, level, health, strength, dexterity, intelligence);
                game.AddCharacter(character);

                Console.WriteLine("Персонаж добавлен.");
            }

            private static void RemoveCharacter(Game game)
            {
                Console.Write("Введите имя персонажа для удаления: ");
                string name = Console.ReadLine();

                bool removed = game.RemoveCharacter(name);

                if (removed)
                {
                    Console.WriteLine("Персонаж удален.");
                }
                else
                {
                    Console.WriteLine("Персонаж не найден.");
                }
            }

            private static void AddItemToCharacter(Game game)
            {
                Console.Write("Введите имя персонажа: ");
                string name = Console.ReadLine();

                Console.Write("Введите название предмета: ");
                string itemName = Console.WriteLine();

                Console.Write("Введите тип предмета (например, оружие, броня, зелье): ");
                string type = Console.WriteLine();

                Console.Write("Введите стоимость предмета: ");
                double value = double.parse(Console.ReadLine());

                Console.Write("Введите вес предмета: ");
                int weight = int.parse(Console.ReadLine());

                Item item = new Item(itemName, type, value, weight);
                game.AddItemToInventory(name, item);

                Console.WriteLine("Предмет добавлен в инвентарь.");
            }

            private static void RemoveItemFromCharacter(Game game)
            {
                Console.Write("Введите имя персонажа: ");
                string name = Console.ReadLine();

                Console.Write("Введите название предмета, который нужно удалить: ");
                string itemName = Console.ReadLine();

                bool removed = game.RemoveItemFromInventory(name, itemName);

                if (removed)
                {
                    Console.WriteLine("Предмет удален из инвентаря.");
                }
                else
                {
                    Console.WriteLine("Предмет не найден.");
                }
            }
        }
    }
}