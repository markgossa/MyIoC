using MyIoC.Tests.Unit.Contracts;

namespace MyIoC.Tests.Unit.Models
{
    internal class EngineMapper : IMapper<IEngine, EngineInfo>
    {
        public EngineInfo Map(IEngine input) => new EngineInfo { Shape = input.Shape };
    }
}
