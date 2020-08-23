using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class ClassNestedParams : IClassInterface
    {
        public ClassNoParams Value1 { get; }
        public ClassSingleParam Value2 { get; }

        public ClassNestedParams(ClassNoParams classNoParams1, ClassSingleParam classNoParams2)
        {
            Value1 = classNoParams1;
            Value2 = classNoParams2;
        }
    }
}
