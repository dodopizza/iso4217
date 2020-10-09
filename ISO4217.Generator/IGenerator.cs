using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISO4217.Generator
{
    public interface IGenerator
    {
        Task Generate(Dictionary<string, Currency> currencies);
    }
}