using System;
using System.Collections.Generic;
using System.Linq;

namespace OverwatchChaos
{
    public class OptionSelector
    {
        private readonly Random _random;

        public OptionSelector()
        {
            _random = new Random();
        }
        
        public List<object> Choose(Dictionary<object, object> values)
        {
            var choicePath = new List<object>();

            var previous = values.ElementAtOrDefault(_random.Next(0, values.Count));
            while (true)
            {
                choicePath.Add(previous.Key);
                var dictionary = previous.Value as Dictionary<object, object>;
                if (dictionary == null)
                {
                    object value;
                    if (double.TryParse(previous.Value.ToString(), out double _))
                        value = Math.Round(_random.NextDouble() * (5.0 - 0.1) - 0.1, 2);
                    else
                    if (bool.TryParse(previous.Value.ToString(), out bool b))
                        value = !b;
                    else
                        value = previous.Value;

                    choicePath.Add(value);
                    break;
                }

                int index = _random.Next(0, dictionary.Count - 1);
                previous = dictionary.ElementAtOrDefault(index);
            }

            return choicePath;
        }

        public Dictionary<object, object> ChooseMany(Dictionary<object, object> values, int count)
        {
            var choices = new Dictionary<object, object>();

            int hangupCounter = 0;
            for (int n = 0; n < count; n++)
            {
                var path = Choose(values);

                if (!choices.TryGetValue(path.First(), out object value))
                    choices.Add(path.First(), new Dictionary<string, object>());

                var option = choices[path.First()] as Dictionary<string, object>;
                var key = string.Join(":", path.Skip(1).Take(path.Count - 2));
                if (option.ContainsKey(key))
                {
                    if (hangupCounter > 25)
                    {
                        Console.WriteLine($"Ran out of options to generate after {n} iterations...\n");
                        break;
                    }
                    hangupCounter++;
                    n--;
                    continue;
                }
                option.Add(key, path.Last().ToString());
            }

            return choices;
        }
    }
}
