namespace NoMoney.InputSystem
{
    public interface IBroadcaster
    {
        delegate void Broadcast(InputData input);
        event Broadcast OnBroadcast;

        InputData HandleCollision(InputData highPriority, InputData lowPriority);
    }
}
