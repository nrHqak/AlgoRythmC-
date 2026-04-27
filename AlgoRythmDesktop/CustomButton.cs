using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AlgoRythmDesktop
{
    public class CustomButton : Button
    {
        private Color _startColor = ColorTranslator.FromHtml("#7C3AED"); // Accent1Color
        private Color _endColor = ColorTranslator.FromHtml("#5A2DBA"); // Darker purple
        private Color _hoverStartColor = ColorTranslator.FromHtml("#8E4DFB");
        private Color _hoverEndColor = ColorTranslator.FromHtml("#6B3ECB");

        public CustomButton(string text)
        {
            this.Text = text;
            this.ForeColor = ColorTranslator.FromHtml("#F1F5F9"); // TextPrimaryColor
            this.Font = new Font("Segoe UI Semibold", 10F);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(100, 30);
            this.Cursor = Cursors.Hand;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath path = new GraphicsPath();
            int radius = 8;
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseAllFigures();

            this.Region = new Region(path);

            Color currentStartColor = this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)) ? _hoverStartColor : _startColor;
            Color currentEndColor = this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)) ? _hoverEndColor : _endColor;

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, currentStartColor, currentEndColor, LinearGradientMode.Vertical))
            {
                g.FillPath(brush, path);
            }

            TextRenderer.DrawText(g, this.Text, this.Font, rect, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Invalidate();
        }
    }
}
