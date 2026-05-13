using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW4_BeepPlayer
{
    public class frmBeepPlayer : Form
    {
        private readonly int[] freq = { 523, 587, 659, 698, 784, 880, 988, 1046 };
        private readonly string[] names = { "Do", "Re", "Mi", "Fa", "Sol", "La", "Si", "Do" };

        private Panel palMain;
        private int initWidth;
        private int initHeight;
        private Dictionary<string, Rectangle> initControl = new Dictionary<string, Rectangle>();

        private Panel pnlInstrument;
        private Label lblInstrument;
        private Timer instrumentTimer;
        private bool isHitDown = false;
        private int animationCount = 0;

        public frmBeepPlayer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "簡易電子琴";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(790, 190);
            this.MinimumSize = new Size(700, 170);
            this.Load += frmBeepPlayer_Load;
            this.SizeChanged += frmBeepPlayer_SizeChanged;

            palMain = new Panel();
            palMain.Dock = DockStyle.Fill;
            palMain.Padding = new Padding(12);
            this.Controls.Add(palMain);

            for (int i = 0; i < 8; i++)
            {
                Button btn = new Button();
                btn.Name = "btn" + (i + 1);
                btn.Text = names[i];
                btn.TabIndex = i;
                btn.Location = new Point(16 + i * 65, 55);
                btn.Size = new Size(54, 62);
                btn.Click += btn1_Click;
                palMain.Controls.Add(btn);
            }

            pnlInstrument = new Panel();
            pnlInstrument.Name = "pnlInstrument";
            pnlInstrument.Location = new Point(560, 18);
            pnlInstrument.Size = new Size(170, 145);
            pnlInstrument.BackColor = Color.FromArgb(245, 245, 245);
            pnlInstrument.BorderStyle = BorderStyle.FixedSingle;
            pnlInstrument.Paint += pnlInstrument_Paint;
            palMain.Controls.Add(pnlInstrument);

            lblInstrument = new Label();
            lblInstrument.Name = "lblInstrument";
            lblInstrument.Text = "按鍵時敲鼓";
            lblInstrument.TextAlign = ContentAlignment.MiddleCenter;
            lblInstrument.Location = new Point(25, 118);
            lblInstrument.Size = new Size(120, 22);
            pnlInstrument.Controls.Add(lblInstrument);

            instrumentTimer = new Timer();
            instrumentTimer.Interval = 90;
            instrumentTimer.Tick += instrumentTimer_Tick;
        }

        private async void btn1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            btn.Enabled = false;
            StartInstrumentAnimation(8);

            int index = btn.TabIndex;
            await Task.Run(() => PlayTone(freq[index], 300));

            btn.Enabled = true;
        }

        // 不使用 Windows Beep，改成自己產生 WAV 聲音，比較不會發生按了沒聲音的問題
        private void PlayTone(int frequency, int durationMs)
        {
            int sampleRate = 44100;
            short bitsPerSample = 16;
            short channels = 1;
            int samples = sampleRate * durationMs / 1000;
            int bytesPerSample = bitsPerSample / 8;
            int dataSize = samples * channels * bytesPerSample;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(new char[] { 'R', 'I', 'F', 'F' });
                bw.Write(36 + dataSize);
                bw.Write(new char[] { 'W', 'A', 'V', 'E' });
                bw.Write(new char[] { 'f', 'm', 't', ' ' });
                bw.Write(16);
                bw.Write((short)1);
                bw.Write(channels);
                bw.Write(sampleRate);
                bw.Write(sampleRate * channels * bytesPerSample);
                bw.Write((short)(channels * bytesPerSample));
                bw.Write(bitsPerSample);
                bw.Write(new char[] { 'd', 'a', 't', 'a' });
                bw.Write(dataSize);

                for (int i = 0; i < samples; i++)
                {
                    double t = (double)i / sampleRate;
                    short value = (short)(Math.Sin(2 * Math.PI * frequency * t) * short.MaxValue * 0.25);
                    bw.Write(value);
                }

                bw.Flush();
                ms.Position = 0;

                using (SoundPlayer sp = new SoundPlayer(ms))
                {
                    sp.PlaySync();
                }
            }
        }

        private void frmBeepPlayer_Load(object sender, EventArgs e)
        {
            initWidth = palMain.Width;
            initHeight = palMain.Height;
            initControl.Clear();

            foreach (Control ctl in palMain.Controls)
            {
                initControl[ctl.Name] = new Rectangle(ctl.Left, ctl.Top, ctl.Width, ctl.Height);
            }
        }

        private void StartInstrumentAnimation(int count)
        {
            animationCount = count;
            isHitDown = false;
            instrumentTimer.Start();
            pnlInstrument.Invalidate();
        }

        private void instrumentTimer_Tick(object sender, EventArgs e)
        {
            isHitDown = !isHitDown;
            animationCount--;
            pnlInstrument.Invalidate();

            if (animationCount <= 0)
            {
                instrumentTimer.Stop();
                isHitDown = false;
                pnlInstrument.Invalidate();
            }
        }

        private void pnlInstrument_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen blackPen = new Pen(Color.Black, 3))
            using (Pen stickPen = new Pen(Color.SaddleBrown, 5))
            using (Brush drumBrush = new SolidBrush(Color.LightGray))
            using (Brush drumSideBrush = new SolidBrush(Color.Silver))
            using (Brush redBrush = new SolidBrush(Color.IndianRed))
            {
                g.FillEllipse(drumBrush, 42, 74, 86, 28);
                g.DrawEllipse(blackPen, 42, 74, 86, 28);
                g.FillRectangle(drumSideBrush, 42, 88, 86, 22);
                g.DrawRectangle(blackPen, 42, 88, 86, 22);
                g.FillEllipse(drumBrush, 42, 96, 86, 28);
                g.DrawEllipse(blackPen, 42, 96, 86, 28);

                if (isHitDown)
                {
                    g.DrawLine(stickPen, 35, 25, 72, 78);
                    g.DrawLine(stickPen, 135, 25, 98, 78);
                    g.FillEllipse(redBrush, 67, 74, 10, 10);
                    g.FillEllipse(redBrush, 94, 74, 10, 10);
                }
                else
                {
                    g.DrawLine(stickPen, 30, 18, 68, 53);
                    g.DrawLine(stickPen, 140, 18, 102, 53);
                    g.FillEllipse(redBrush, 63, 49, 10, 10);
                    g.FillEllipse(redBrush, 98, 49, 10, 10);
                }
            }
        }

        private void frmBeepPlayer_SizeChanged(object sender, EventArgs e)
        {
            if (initWidth == 0 || initHeight == 0 || initControl.Count == 0) return;

            double ratioWidth = (double)palMain.Width / initWidth;
            double ratioHeight = (double)palMain.Height / initHeight;

            foreach (Control ctl in palMain.Controls)
            {
                Rectangle r = initControl[ctl.Name];
                ctl.Left = (int)(r.Left * ratioWidth);
                ctl.Top = (int)(r.Top * ratioHeight);
                ctl.Width = Math.Max(30, (int)(r.Width * ratioWidth));
                ctl.Height = Math.Max(30, (int)(r.Height * ratioHeight));
            }
        }
    }
}
