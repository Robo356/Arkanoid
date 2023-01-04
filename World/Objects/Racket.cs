using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Objects
{
    public class Racket : Thing, IBoundsThing, ISolidThing, IMovingThing
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public float SpeedX { get; set; }
        public float SpeedY
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        public Color Color
        {
            get { return _ColorBrush.Color; }
            set { _ColorBrush = new SolidBrush(value); }
        }

        private SolidBrush _ColorBrush;

        public event CollideEventHandler OnCollide;

        public Racket(World world, Color color, float x, float y, float width, float height)
            : base(world)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
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
            var bounds = Bounds;
            var worldBounds = _World.Bounds;
            if (bounds.Right > worldBounds.Right)
            {
                X = worldBounds.Right - Width;
            }
            else if (bounds.Left < worldBounds.Left)
            {
                X = worldBounds.Left;
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(_ColorBrush, X, Y, Width, Height);
        }
    }
}
