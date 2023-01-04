
namespace World.Objects
{
    public class Ball : Thing, IMovingThing, ISolidThing
    {
        private SolidBrush _ColorBrush;
        public float Radius { get; set; }
        public Color Color
        {
            get { return _ColorBrush.Color; }
            set { _ColorBrush = new SolidBrush(value); }
        }
        public float SpeedX { get; set; }
        public float SpeedY { get; set; }

        public Ball(World world, Color color, float x, float y, float radius, float speedX, float speedY)
            : base(world)
        {
            Color = color;
            X = x;
            Y = y;
            Radius = radius;
            SpeedX = speedX;
            SpeedY = speedY;
        }

        public RectangleF Bounds
        {
            get { return new RectangleF(X - Radius, Y - Radius, Radius * 2, Radius * 2); }
        }

        public void Collide(ISolidThing otherThing)
        {
            if (otherThing is Ball ball)
            {
                // Correct collision for ball
                double dist = Math.Pow(X - ball.X, 2) + Math.Pow(Y - ball.Y, 2); // squared distance
                double radius = Math.Pow(Radius + ball.Radius, 2);
                if (dist < radius)
                {

                }
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color), X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }

        public void ExitWorld()
        {
            // Bounce left, right, top
            var worldBounds = _World.Bounds;
            var bounds = Bounds;
            if (bounds.Left < worldBounds.Left || bounds.Right > worldBounds.Right)
            {
                SpeedX = -SpeedX;
            }
            if (bounds.Top < worldBounds.Top)
            {
                SpeedY = -SpeedY;
            }
            if (bounds.Bottom > worldBounds.Bottom)
            {
                SpeedX = SpeedY = 0;
                Color = Color.Red;
            }
        }
    }
}
