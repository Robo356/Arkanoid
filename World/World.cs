using World.Objects;

namespace World
{
    public class World : IDisposable
    {
        public List<Thing> Things = new List<Thing>();
        public List<Thing> ThingsToBeRemoved = new List<Thing>();
        protected Bitmap _Canvas;
        protected Color _BackgroundColor;
        protected RectangleF _Bounds;
        protected bool _DebugInfo;

        public World(int width, int height)
        {
            _DebugInfo = false;
            _BackgroundColor = Color.WhiteSmoke;
            ResetCanvas(width, height);
        }

        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; }
        }

        public RectangleF Bounds
        {
            get { return _Bounds; }
        }

        public void ResetCanvas(int width, int height)
        {
            if (_Canvas != null)
            {
                _Canvas.Dispose();
            }
            _Canvas = new Bitmap(width, height);
            _Bounds = new RectangleF(0, 0, _Canvas.Width, _Canvas.Height);
        }

        public void Update()
        {
            var worldBounds = Bounds;
            foreach (var thing in Things)
            {
                if (thing.LastUpdate > 0)
                {
                    var interval = new TimeSpan(DateTime.Now.Ticks - thing.LastUpdate);
                    if (thing is IMovingThing movingThing)
                    {
                        // Update coordinates
                        thing.X += (movingThing.SpeedX * interval.Milliseconds) / 1000;
                        thing.Y += (movingThing.SpeedY * interval.Milliseconds) / 1000;
                    }
                    if (thing is IBoundsThing boundsThing)
                    {
                        var bounds = boundsThing.Bounds;
                        if (thing is ISolidThing solidThing)
                        {
                            // Collision detection
                            foreach (var otherThing in Things)
                            {
                                if (otherThing != thing && otherThing is ISolidThing otherSolidThing)
                                {
                                    var otherBounds = otherSolidThing.Bounds;
                                    if (bounds.IntersectsWith(otherBounds))
                                    {
                                        solidThing.Collide(otherSolidThing);
                                    }
                                }
                            }
                        }
                        // Exit detection
                        if (bounds.Left <= worldBounds.Left || bounds.Top <= worldBounds.Top ||
                            bounds.Right >= worldBounds.Right || bounds.Bottom >= worldBounds.Bottom)
                        {
                            boundsThing.ExitWorld();
                        }
                    }
                    // Custom Thing update
                    thing.Update(interval);
                }
                thing.LastUpdate = DateTime.Now.Ticks;
            }
            // Things scheduled to be removed
            foreach (var thing in ThingsToBeRemoved)
            {
                Things.Remove(thing);
            }
            ThingsToBeRemoved.Clear();
        }

        public void DrawOnCanvas()
        {
            using (var g = Graphics.FromImage(_Canvas))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                // Draw background
                g.FillRectangle(new SolidBrush(_BackgroundColor), Bounds);
                // Draw all things
                foreach (var thing in Things)
                {
                    thing.Draw(g);
                    if (_DebugInfo)
                    {
                        thing.DrawInfo(g);
                    }
                }
            }
        }

        public void Paint(Graphics g, float x, float y)
        {
            g.DrawImage(_Canvas, x, y, _Canvas.Width, _Canvas.Height);
        }

        public void Dispose()
        {
            if (_Canvas != null)
            {
                _Canvas.Dispose();
                _Canvas = null;
            }
        }
    }
}
