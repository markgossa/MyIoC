using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class SparkPlug : ISparkPlug
    {
        public int Size => 10;
        private readonly ICylinder _cylinder;

        public SparkPlug(ICylinder cylinder)
        {
            _cylinder = cylinder;
        }
    }
}
