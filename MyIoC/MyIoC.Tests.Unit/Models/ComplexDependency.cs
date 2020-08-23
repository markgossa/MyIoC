using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class ComplexDependency : IComplexDependency
    {
        private readonly IClassInterface _classInterface;

        public ComplexDependency(IClassInterface classInterface)
        {
            _classInterface = classInterface;
        }
    }
}
