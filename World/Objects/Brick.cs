namespace World.Objects
{
    public class Brick : Thing, ISolidThing
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public event CollideEventHandler OnCollide;

        public Color Color 
        {
            get { return _ColorBrush.Color; }
            set { _ColorBrush = new SolidBrush(value);  }
        }

        public int Points
        {
            get { return _Points; }
        }

        private float _BorderWidth;
        private SolidBrush _ColorBrush;
        private SolidBrush _ShadowBrush;
        private SolidBrush _LightBrush;
        private int _Points;

        public Brick(World world, Color color, int points, float x, float y, float width, float height)
            : base(world)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _BorderWidth = 1;
            _ColorBrush = new SolidBrush(color);
            _ShadowBrush = new SolidBrush(Color.Black);
            _LightBrush = new SolidBrush(Color.WhiteSmoke);
            _Points = points;
        }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X, Y, Width, Height); 
            }
        }

        public void Collide(ISolidThing otherThing)
        {
            OnCollide(this, new CollideEventArgs(this, otherThing));
        }

        public void ExitWorld()
        {
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(_ColorBrush, X, Y, Width, Height);
            g.FillRectangle(_LightBrush, X + 4, Y + 3, Width - 10, 3);
            g.FillRectangle(_ShadowBrush, X, Y + Height - _BorderWidth, Width, _BorderWidth);
            g.FillRectangle(_ShadowBrush, X + Width - _BorderWidth, Y, _BorderWidth, Height);
        }
    }
}
