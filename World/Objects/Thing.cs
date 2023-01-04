
namespace World.Objects
{
    public abstract class Thing
    {
        protected static readonly Font InfoFont = new Font(FontFamily.GenericSansSerif, 10F, FontStyle.Regular);
        protected static readonly Brush InfoBrush = Brushes.Black;

        protected World _World;

        public Thing(World world)
        {
            _World = world;
        }

        internal long LastUpdate { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public virtual void DrawInfo(Graphics g)
        {
            float x, y;
            if (this is IBoundsThing boundsThing)
            {
                var bounds = boundsThing.Bounds;
                x = bounds.X;
                y = bounds.Y;
            }
            else
            {
                x = X;
                y = Y;
            }
            var lineSize = g.MeasureString($"X: {X:F2}", InfoFont);
            g.DrawString($"X: {X:F2}", InfoFont, InfoBrush, x, y);
            g.DrawString($"Y: {Y:F2}", InfoFont, InfoBrush, x, y + lineSize.Height);
        }

        public abstract void Draw(Graphics g);

        public virtual void Update(TimeSpan interval)
        {

        }
    }
}
