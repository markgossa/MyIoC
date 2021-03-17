using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class Hatchback : ICar
    {
        private readonly IEngine _engine;

        public Hatchback(IEngine engine)
        {
            _engine = engine;
        }

        public IEngine GetEngine() => _engine;
    }
}
