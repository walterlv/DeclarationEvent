// ReSharper disable InconsistentNaming
namespace Walterlv.Events
{
    /// <summary>
    /// Declaration Chain builder.
    /// </summary>
    public sealed class DC
    {
        private readonly DeclarationChain _chain = new DeclarationChain();

        public DC Down(params DE[] infos)
        {
            _chain.Add(new DownChainNode(infos));
            return this;
        }

        public DC Up(params DE[] infos)
        {
            _chain.Add(new UpChainNode(infos));
            return this;
        }

        internal DeclarationChain ToChain()
        {
            return _chain;
        }
    }
}
