using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class ComponentBuilder<TOut> : IBuilder<TOut>
    {
        public TOut Build(TOut output) => output;
    }
}
