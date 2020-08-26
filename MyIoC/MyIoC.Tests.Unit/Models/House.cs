using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    public class House : IBuilding
    {
        public Room Room { get; }

        public House(Room room)
        {
            Room = room;
        }
    }
}