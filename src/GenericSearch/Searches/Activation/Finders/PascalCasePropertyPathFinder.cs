using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GenericSearch.Exceptions;

namespace GenericSearch.Searches.Activation.Finders
{
    public class PascalCasePropertyPathFinder : IPropertyPathFinder
    {
        private readonly Regex expression = new Regex(@"(\p{Lu}+(?=$|\p{Lu}[\p{Ll}0-9])|\p{Lu}?[\p{Ll}0-9]+)");

        private static string ReplaceValue(Match match) =>
            match.Value[0].ToString().ToUpper() + match.Value.Substring(1);

        public string Find(Type entityType, string source)
        {
            var path = new StringBuilder();
            var matches = expression.Matches(source).Select(ReplaceValue).ToArray();

            while (matches.Any())
            {
                for (var i = 1; i <= matches.Length; i++)
                {
                    var name = string.Join("", matches.Take(i));
                    var match = entityType.GetProperty(name);
                    if (match != null)
                    {
                        entityType = match.PropertyType;
                        if (path.Length > 0)
                        {
                            path.Append(".");
                        }

                        path.Append(name);
                        matches = matches.Skip(i).ToArray();
                        continue;
                    }

                    if (i == matches.Length)
                    {
                        return null;
                        //throw new PropertyNotFoundException($"No property for {string.Join("", matches)} was found.");
                    }
                }
            }

            return path.ToString();
        }
    }
}