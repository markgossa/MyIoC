using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class ClassSingleParam : IClassInterface
    {
        public ClassNoParams Value { get; }

        public ClassSingleParam(ClassNoParams testClass1)
        {
            Value = testClass1;
        }
    }
}
