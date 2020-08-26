using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class VEngine : IEngine
    {
        public ICylinder Cylinder { get; }
        public ISparkPlug SparkPlug { get; }
        public const string Shape = "V";

        public VEngine(ICylinder cylinder, ISparkPlug sparkPlug)
        {
            Cylinder = cylinder;
            SparkPlug = sparkPlug;
        }
    }
}
