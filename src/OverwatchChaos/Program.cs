using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace OverwatchChaos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var filepath = Path.Combine(AppContext.BaseDirectory, "default_heroes.yml");
            var content = File.ReadAllText(filepath);
            
            var yamlreader = new DeserializerBuilder().Build();
            var yamlwriter = new SerializerBuilder().Build();
            var heroes = yamlreader.Deserialize<Dictionary<object, object>>(content);

            Console.WriteLine($"Loaded {heroes.Count} heroes from config...\n");
            Console.WriteLine($"Available heroes: {string.Join(", ", heroes.Select(x => x.Key))}\n");
            var selector = new OptionSelector();
            while (true)
            {
                Console.WriteLine("Who should I generate options for? (press enter to skip) ");
                var generateHeroes = Console.ReadLine().ToLower().Split(',', ' ').ToList();

                var validHeroes = heroes.Select(x => x.Key);
                generateHeroes.RemoveAll(x => !validHeroes.Contains(x));

                Dictionary<object, object> chosenHeroes;
                if (generateHeroes.Count == 0)
                {
                    chosenHeroes = heroes;
                    Console.WriteLine($"Generating options for: all\n");
                }
                else
                {
                    chosenHeroes = heroes.Where(x => generateHeroes.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                    Console.WriteLine($"Generating options for: {string.Join(", ", chosenHeroes.Select(x => x.Key))}\n");
                }

                Console.WriteLine($"How many options should I generate? ");
                var input = Console.ReadLine();
                Console.WriteLine();
                if (!int.TryParse(input, out int number))
                {
                    Console.WriteLine($"{input} is not a valid number, try again!");
                    continue;
                }

                var choices = selector.ChooseMany(chosenHeroes, number);
                Console.WriteLine(yamlwriter.Serialize(choices));
                Console.WriteLine("Generate more? y/n");
                var more = Console.ReadLine();
                if (more.ToLower() == "n") break;
            }
        }
    }
}
