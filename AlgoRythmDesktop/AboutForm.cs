using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoRythmDesktop
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "О программе AlgoRythm";
            this.BackColor = ColorTranslator.FromHtml("#1A1A2E"); // PanelColor

            Label titleLabel = new Label();
            titleLabel.Text = "AlgoRythm Desktop";
            titleLabel.Font = new Font("Segoe UI Semibold", 16F);
            titleLabel.ForeColor = ColorTranslator.FromHtml("#F1F5F9"); // TextPrimaryColor
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(20, 20);
            this.Controls.Add(titleLabel);

            Label versionLabel = new Label();
            versionLabel.Text = "Версия: 1.0";
            versionLabel.Font = new Font("Segoe UI", 10F);
            versionLabel.ForeColor = ColorTranslator.FromHtml("#94A3B8"); // TextSecondaryColor
            versionLabel.AutoSize = true;
            versionLabel.Location = new Point(20, 50);
            this.Controls.Add(versionLabel);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = "AlgoRythm — это интерактивный визуализатор алгоритмов сортировки и поиска для школьников и студентов. Пользователь вводит массив чисел, выбирает алгоритм, и наблюдает его работу пошагово в виде анимированных баров с подсказками.";
            descriptionLabel.Font = new Font("Segoe UI", 10F);
            descriptionLabel.ForeColor = ColorTranslator.FromHtml("#F1F5F9");
            descriptionLabel.MaximumSize = new Size(360, 0);
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(20, 80);
            this.Controls.Add(descriptionLabel);

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(150, 200);
            okButton.Size = new Size(80, 30);
            okButton.Click += (sender, e) => this.Close();
            this.Controls.Add(okButton);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AboutForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Name = "AboutForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
