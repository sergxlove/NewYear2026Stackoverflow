using System.Drawing.Drawing2D;

namespace NewYear2026Stackoverflow
{
    public partial class Form1 : Form
    {
        private readonly List<Snowflake> snowflakes = new List<Snowflake>();
        private readonly Random random = new Random();
        private readonly System.Windows.Forms.Timer animationTimer;
        public Form1()
        {
            InitializeComponent();

            Text = "С Новым Годом!";
            ClientSize = new Size(1350, 830);
            BackColor = Color.MidnightBlue;
            DoubleBuffered = true;
            for (int i = 0; i < 500; i++)
            {
                snowflakes.Add(CreateRandomSnowflake());
            }
            animationTimer = new System.Windows.Forms.Timer { Interval = 1000 / 60 };
            animationTimer.Tick += (s, e) => { UpdateSnowflakes(); Invalidate(); };
            animationTimer.Start();
            Paint += MainForm_Paint!;
        }

        private Snowflake CreateRandomSnowflake()
        {
            return new Snowflake
            {
                X = random.Next(ClientSize.Width),
                Y = random.Next(-100, 0), 
                Size = random.Next(3, 12),
                SpeedY = 1 + random.NextDouble() * 3,
                SpeedX = random.NextDouble() - 0.5, 
                Opacity = 0.3f + (float)random.NextDouble() * 0.7f,
                Color = Color.FromArgb(random.Next(200, 256), random.Next(200, 256), 255) 
            };
        }

        private void UpdateSnowflakes()
        {
            for (int i = 0; i < snowflakes.Count; i++)
            {
                var flake = snowflakes[i];
                flake.Y += flake.SpeedY;
                flake.X += flake.SpeedX;
                if (flake.Y > ClientSize.Height ||
                    flake.X < -10 || flake.X > ClientSize.Width + 10)
                {
                    snowflakes[i] = CreateRandomSnowflake();
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            foreach (var flake in snowflakes)
            {
                using var brush = new SolidBrush(Color.FromArgb((int)(flake.Opacity * 255), flake.Color));
                g.FillEllipse(brush, (float)flake.X, (float)flake.Y, flake.Size, flake.Size);
            }
            using (var font = new Font("Comic Sans MS", 32, FontStyle.Bold))
            using (var textBrush = new LinearGradientBrush(
                new Point(0, 0), new Point(this.ClientSize.Width, 0),
                Color.Red, Color.Gold))
            {
                string text = "С НАСТУПАЮЩИМ НОВЫМ ГОДОМ!";
                var textSize = g.MeasureString(text, font);
                g.DrawString(text, font, textBrush,
                    (this.ClientSize.Width - textSize.Width) / 2, 20);
            }
        }
    }
}
