using World.Objects;

namespace World
{
    public partial class FormGame : Form
    {
        private Arkanoid World;
        private Ball TheBall;
        public FormGame()
        {
            InitializeComponent();
            World = new Arkanoid();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            World.Paint(e.Graphics, 0, 0);
        }

        private void UpdateWorldTimer_Tick(object sender, EventArgs e)
        {
            World.Update();
            World.DrawOnCanvas();
            using (var g = CreateGraphics())
            {
                World.Paint(g, 0, 0);
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Right)
            {
                World.MoveRacketRight();
            }
            else if (e.KeyCode== Keys.Left)
            {
                World.MoveRacketLeft();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
            {
                World.StopMovingRacket();
            }
        }
    }
}