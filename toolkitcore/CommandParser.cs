﻿using System.Collections.Generic;

namespace ToolkitCore
{
    public static class CommandParser
    {
        public static string[] Parse(string input, string prefix = "!")
        {
            if(input.StartsWith(prefix))
            {
                input = input.Substring(prefix.Length);
            }

            var cache = new List<string>();
            var segment = "";
            var quoted = false;
            var escaped = false;

            foreach(var c in input)
            {
                if(c.Equals(' ') && !quoted)
                {
                    cache.Add(segment);
                    segment = "";
                }
                else if(c.Equals('"'))
                {
                    if(!escaped)
                    {
                        quoted = !quoted;
                    }
                    else
                    {
                        segment += c;
                        escaped = false;
                    }
                }
                else if(c.Equals('\\'))
                {
                    escaped = true;
                }
                else
                {
                    segment += c;
                }
            }

            if(segment.Length > 0)
            {
                cache.Add(segment);
            }

            return cache.ToArray();
        }

        public static List<KeyValuePair<string, string>> ParseKeyed(string input, string prefix = "!")
        {
            return ParseKeyed(Parse(input, prefix: prefix));
        }

        public static List<KeyValuePair<string, string>> ParseKeyed(string[] input)
        {
            var cache = new List<KeyValuePair<string, string>>();

            foreach(var segment in input)
            {
                if(segment.Contains("="))
                {
                    var key = "";
                    var value = "";
                    var sep = false;
                    var escaped = false;

                    foreach(var c in segment)
                    {
                        if(c.Equals('=') && !escaped)
                        {
                            escaped = false;
                            sep = true;
                        }
                        else if(c.Equals('\\'))
                        {
                            escaped = true;
                        }
                        else
                        {
                            if(!sep)
                            {
                                key += c;
                            }
                            else
                            {
                                value += c;
                            }
                        }
                    }
                    cache.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            return cache;
        }
    }
}
