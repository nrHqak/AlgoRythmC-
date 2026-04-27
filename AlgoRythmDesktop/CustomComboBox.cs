using System.Drawing;
using System.Windows.Forms;

namespace AlgoRythmDesktop
{
    public class CustomComboBox : ComboBox
    {
        public CustomComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.BackColor = ColorTranslator.FromHtml("#1E1E2E"); // Dark background
            this.ForeColor = ColorTranslator.FromHtml("#F1F5F9"); // Primary text color
            this.Font = new Font("Segoe UI", 10F);
            this.FlatStyle = FlatStyle.Flat;
            this.ItemHeight = 20;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (e.Index < 0) return;

            e.DrawBackground();

            Graphics g = e.Graphics;
            Brush brush;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                brush = new SolidBrush(ColorTranslator.FromHtml("#7C3AED")); // Accent1Color for selected item
            }
            else
            {
                brush = new SolidBrush(this.BackColor);
            }

            g.FillRectangle(brush, e.Bounds);

            g.DrawString(this.Items[e.Index].ToString(), e.Font, new SolidBrush(this.ForeColor), e.Bounds, StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }
    }
}
