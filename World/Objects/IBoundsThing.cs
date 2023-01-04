namespace World.Objects
{
    public interface IBoundsThing
    {
        RectangleF Bounds { get; }
        void ExitWorld();
    }
}
