using System;
using System.Collections.Generic;
using System.IO;
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
            
            var yaml = new DeserializerBuilder().Build();
            var heroes = yaml.Deserialize<Dictionary<object, object>>(content);

            var selector = new OptionSelector();
            while (true)
            {
                string reply = selector.Choose(heroes);

                Console.WriteLine(reply);
                Console.ReadKey();
            }
        }
    }
}
