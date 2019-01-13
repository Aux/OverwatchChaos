using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OverwatchChaos
{
    public class OptionSelector
    {
        private readonly Random _random;

        public OptionSelector()
        {
            _random = new Random();
        }

        public string Choose(Dictionary<object, object> values)
        {
            var builder = new StringBuilder();
            
            var previous = values.ElementAtOrDefault(_random.Next(0, values.Count - 1));
            while (true)
            {
                builder.AppendFormat("{0}:", previous.Key);
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
                    
                    builder.Append("\t" + value);
                    break;
                }
                
                int index = _random.Next(0, dictionary.Count - 1);
                previous = dictionary.ElementAtOrDefault(index);
            }

            return builder.ToString();
        }
    }
}
