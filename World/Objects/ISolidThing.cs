namespace World.Objects
{
    public class CollideEventArgs
    {
        public CollideEventArgs(ISolidThing thing, ISolidThing other)
        {
            Thing= thing;
            Other = other;
        }

        public readonly ISolidThing Thing;
        public readonly ISolidThing Other;
    }

    public delegate void CollideEventHandler(object sender, CollideEventArgs e);

    public interface ISolidThing : IBoundsThing
    {
        void Collide(ISolidThing otherThing);
    }
}
