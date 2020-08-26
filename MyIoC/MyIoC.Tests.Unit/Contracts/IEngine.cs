using MyIoC.Tests.Unit.Models;

namespace MyIoC.Tests.Unit.Contracts
{
    internal interface IEngine
    {
        ICylinder Cylinder { get; }
        ISparkPlug SparkPlug { get; }
        const string Shape = "V";
    }
}