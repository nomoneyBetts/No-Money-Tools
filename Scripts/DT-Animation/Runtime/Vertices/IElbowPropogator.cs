// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

namespace NoMoney.DTAnimation
{
    /// <summary>
    /// Implemented by vertices who can interact with Elbow Propogation.
    /// </summary>
    public interface IElbowPropogator
    {
        /// <summary>
        /// Calls along the chain of elbows to propogate updates.
        /// </summary>
        /// <param name="propogator">The Vertex calling.</param>
        /// <param name="target">The updated Target.</param>
        /// <param name="port">The updated Port.</param>
        void Propogate(Vertex propogator, Vertex target, string port);
    }
}
