using System.Collections.Generic;

namespace MyIoC.Tests.Unit.Models
{
    internal class Street
    {
        public IEnumerable<House> Houses { get; }

        public Street(IEnumerable<House> houses)
        {
            Houses = houses;
        }
    }
}
