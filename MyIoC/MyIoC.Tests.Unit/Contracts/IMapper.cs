namespace MyIoC.Tests.Unit.Contracts
{
    internal interface IMapper<TIn, TOut>
    {
        TOut Map(TIn input);
    }
}
