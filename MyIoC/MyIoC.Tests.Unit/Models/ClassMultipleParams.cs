namespace MyIoC.Tests.Unit.Models
{
    internal class ClassMultipleParams
    {
        public ClassNoParams Value1 { get; }
        public ClassNoParams2 Value2 { get; }

        public ClassMultipleParams(ClassNoParams classNoParams1, ClassNoParams2 classNoParams2)
        {
            Value1 = classNoParams1;
            Value2 = classNoParams2;
        }
    }
}
