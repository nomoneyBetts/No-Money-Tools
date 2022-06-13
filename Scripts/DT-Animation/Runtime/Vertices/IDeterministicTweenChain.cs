namespace NoMoney.DTAnimation
{
    /// <summary>
    /// Implemented by Vertices who exist on the tween chain with deterministic outputs.
    /// </summary>
    public interface IDeterministicTweenChain
    {
        /// <summary>
        /// The deterministic output connection.
        /// </summary>
        public Connection OutputCnx { get; }
    }
}
