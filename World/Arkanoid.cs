using System.Diagnostics;
using World.Objects;

namespace World
{
    public class Arkanoid : World
    {
        internal class BrickType
        {
            public readonly Color Color;
            public readonly int Points;

            public BrickType(Color color, int points)
            {
                Color = color;
                Points = points;
            }
        }

        private const float BrickWidth = 40;
        private const float BrickHeight = 20;
        private const float Spacing = 2;
        private const float RacketYOffset = 300;
        private const float RacketWidth = 80;
        private const float RacketHeight = 10;
        private const float RacketSpeed = 300;
        private const float RacketGrip = 0.5F;
        private static readonly Color RacketColor = Color.DarkBlue;
        private Racket _Racket;

        private const float BallRadius = 10;
        private const int BallSpeed = 200;
        private static readonly Color BallColor = Color.SteelBlue;
        private Ball _Ball;

        private int TotalPoints;
        private int TotalBricks;

        static readonly List<BrickType> BrickTypes = new List<BrickType>
        {
            new BrickType(Color.Yellow, 200),
            new BrickType(Color.Purple, 180),
            new BrickType(Color.Blue, 160),
            new BrickType(Color.Red, 140),
            new BrickType(Color.Green, 120),
        };

        public Arkanoid() 
            : base((int)(BrickWidth + Spacing) * 13, 600)
        {
            TotalPoints = TotalBricks = 0;
            float y = BrickHeight;
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                CreateRow(y, random);
                y += BrickHeight + Spacing;
            }

            y += RacketYOffset;
            _Racket = new Racket(this, RacketColor, Bounds.Width / 2 - RacketWidth / 2, y, RacketWidth, RacketHeight);
            _Racket.OnCollide += Racket_OnCollide;
            Things.Add(_Racket);

            _Ball = new Ball(this, BallColor, Bounds.Width / 2, y - BallRadius, BallRadius, random.Next(-BallSpeed, BallSpeed), -BallSpeed);
            Things.Add(_Ball);
        }

        private void Racket_OnCollide(object sender, CollideEventArgs e)
        {
            if (e.Other is Ball ball)
            {
                ball.SpeedX = ball.SpeedX + (_Racket.SpeedX * RacketGrip);
                ball.SpeedY = -ball.SpeedY;
            }
        }

        private void CreateRow(float y, Random random)
        {
            float x = BrickWidth;
            for (int i = 0; i < 3; i++)
            {
                var brickType = BrickTypes[random.Next(BrickTypes.Count)];
                for (int j = 0; j < 3; j++)
                {
                    var brick = new Brick(this, brickType.Color, brickType.Points, x, y, BrickWidth, BrickHeight);
                    brick.OnCollide += Brick_OnCollide;
                    Things.Add(brick);
                    TotalBricks++;
                    x += BrickWidth + Spacing;
                }
                x += BrickWidth;
            }
        }

        private void Brick_OnCollide(object sender, CollideEventArgs e)
        {
            if (e.Other is Ball ball)
            {
                var brick = e.Thing as Brick;
                var brickBounds = brick.Bounds;
                var ballBounds = ball.Bounds;
                var bottomDist = brickBounds.Bottom - ballBounds.Top;
                var topDist = ballBounds.Bottom - brickBounds.Top;
                var leftDist = ballBounds.Right - brickBounds.Left;
                var rightDist = brickBounds.Right - ballBounds.Left;

                if (bottomDist < topDist && bottomDist < leftDist && bottomDist < rightDist)
                {
                    // Hit from bottom
                    _Ball.SpeedY = -_Ball.SpeedY;
                    DestroyBrick(brick);
                }
                else if (topDist < bottomDist && topDist < leftDist && topDist < rightDist)
                {
                    // Hit from top
                    _Ball.SpeedY = -_Ball.SpeedY;
                }
                else if (rightDist < bottomDist && rightDist < leftDist && rightDist < topDist)
                {
                    // Hit from right
                    _Ball.SpeedX = -_Ball.SpeedX;
                }
                else if (leftDist < bottomDist && leftDist < rightDist && leftDist < topDist)
                {
                    // Hit from left
                    _Ball.SpeedX = -_Ball.SpeedX;
                }

            }
        }

        private void DestroyBrick(Brick brick)
        {
                TotalPoints += brick.Points;
                TotalBricks--;
                ThingsToBeRemoved.Add(brick);


                if (TotalBricks == 0)
                {
                    // End all
                    _Ball.SpeedX = _Ball.SpeedY = 0;
                }
        }

        public void MoveRacketRight()
        {
            _Racket.SpeedX = RacketSpeed;
        }

        public void MoveRacketLeft()
        {
            _Racket.SpeedX = -RacketSpeed;
        }

        public void StopMovingRacket()
        {
            _Racket.SpeedX = 0;
        }
    }
}
