namespace MyIoC.Tests.Unit.Contracts
{
    internal interface IBuilder<TOut>
    {
        TOut Build(TOut output) => output;
    }
}
