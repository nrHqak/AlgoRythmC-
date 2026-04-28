using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace AlgoRythmDesktop
{
    public partial class Form1 : Form
    {
        // Color Palette
        private static readonly Color BackgroundColor = ColorTranslator.FromHtml("#0D0D0F");
        private static readonly Color PanelColor = ColorTranslator.FromHtml("#1A1A2E");
        private static readonly Color Accent1Color = ColorTranslator.FromHtml("#7C3AED"); // Purple (Compare)
        private static readonly Color Accent2Color = ColorTranslator.FromHtml("#10B981"); // Green (Swap/Found)
        private static readonly Color Accent3Color = ColorTranslator.FromHtml("#EF4444"); // Red (Error)
        private static readonly Color TextPrimaryColor = ColorTranslator.FromHtml("#F1F5F9");
        private static readonly Color TextSecondaryColor = ColorTranslator.FromHtml("#94A3B8");
        private static readonly Color BarNormalColor = ColorTranslator.FromHtml("#3B4A6B");
        private static readonly Color BarFoundColor = ColorTranslator.FromHtml("#F59E0B"); // Golden

        // Fonts
        private static readonly Font HeaderFont = new Font("Segoe UI Semibold", 14F);
        private static readonly Font BodyFont = new Font("Segoe UI", 10F);
        private static readonly Font CodeFont = new Font("Consolas", 10F);
        private static readonly Font BarLabelFont = new Font("Consolas", 9F);

        // Custom Title Bar
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private Panel titleBarPanel;
        private Label titleLabel;
        private Button closeButton;
        private Button minimizeButton;

        // UI Elements
        private Panel mainPanel;
        private Panel topControlsPanel;
        private Panel visualizationPanel;
        private Panel logPanel;
        private Panel hintPanel;

        private TextBox arrayInputTextBox;
        private Button randomArrayButton;
        private ComboBox algorithmComboBox;
        private Label speedLabel;
        private TrackBar speedTrackBar;
        private Button startButton;
        private Button stepButton;
        private Button stopButton;
        private Button resetButton;
        private PictureBox visualizationPictureBox;
        private ListBox logListBox;
        private Label hintLabel;
        private Label complexityLabel;
        private Label stepsLabel;

        private const int ControlsPadding = 10;
        private const int ControlsSpacing = 10;

        // Algorithm related
        private List<Algorithm> algorithms;
        private Algorithm currentAlgorithm;
        private List<TraceStep> currentTrace;
        private int currentStepIndex;
        private int[] initialArray;
        private System.Windows.Forms.Timer animationTimer;
        private bool isRunning;
        private int searchValue; // For search algorithms

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // Remove default title bar
            this.BackColor = BackgroundColor; // Background color
            this.MinimumSize = new Size(800, 600);

            InitializeCustomTitleBar();
            InitializeLayoutPanels();
            InitializeAlgorithms();
            InitializeControls();
            ResetApplication();
        }

        private void InitializeCustomTitleBar()
        {
            titleBarPanel = new Panel();
            titleBarPanel.Dock = DockStyle.Top;
            titleBarPanel.Height = 30;
            titleBarPanel.BackColor = PanelColor;
            titleBarPanel.MouseDown += TitleBarPanel_MouseDown;
            this.Controls.Add(titleBarPanel);

            titleLabel = new Label();
            titleLabel.Text = "🎵 AlgoRythm";
            titleLabel.ForeColor = TextPrimaryColor;
            titleLabel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            titleLabel.Dock = DockStyle.Left;
            titleLabel.AutoSize = true;
            titleLabel.Padding = new Padding(5, 5, 0, 0);
            titleBarPanel.Controls.Add(titleLabel);

            Button aboutButton = new Button();
            aboutButton.Text = "О программе";
            aboutButton.ForeColor = TextPrimaryColor;
            aboutButton.BackColor = Color.Transparent;
            aboutButton.FlatAppearance.BorderSize = 0;
            aboutButton.FlatStyle = FlatStyle.Flat;
            aboutButton.Dock = DockStyle.Right;
            aboutButton.Width = 100;
            aboutButton.Click += AboutButton_Click;
            titleBarPanel.Controls.Add(aboutButton);

            minimizeButton = new Button();
            minimizeButton.Text = "—";
            minimizeButton.ForeColor = TextPrimaryColor;
            minimizeButton.BackColor = Color.Transparent;
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.FlatStyle = FlatStyle.Flat;
            minimizeButton.Dock = DockStyle.Right;
            minimizeButton.Width = 30;
            minimizeButton.Click += (sender, e) => this.WindowState = FormWindowState.Minimized;
            titleBarPanel.Controls.Add(minimizeButton);

            closeButton = new Button();
            closeButton.Text = "✕";
            closeButton.ForeColor = TextPrimaryColor;
            closeButton.BackColor = Color.Transparent;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Dock = DockStyle.Right;
            closeButton.Width = 30;
            closeButton.Click += (sender, e) => this.Close();
            titleBarPanel.Controls.Add(closeButton);
        }

        private void InitializeLayoutPanels()
        {
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(10);
            mainPanel.BackColor = BackgroundColor;
            this.Controls.Add(mainPanel);
            mainPanel.BringToFront(); // Ensure it's above the title bar

            topControlsPanel = new Panel();
            topControlsPanel.Dock = DockStyle.Top;
            topControlsPanel.Height = 100;
            topControlsPanel.BackColor = PanelColor;
            topControlsPanel.Padding = new Padding(10);
            topControlsPanel.Resize += (s, e) => LayoutTopControls();
            mainPanel.Controls.Add(topControlsPanel);

            hintPanel = new Panel();
            hintPanel.Dock = DockStyle.Bottom;
            hintPanel.Height = 60;
            hintPanel.BackColor = PanelColor;
            hintPanel.Padding = new Padding(10);
            mainPanel.Controls.Add(hintPanel);

            logPanel = new Panel();
            logPanel.Dock = DockStyle.Right;
            logPanel.Width = 250;
            logPanel.BackColor = PanelColor;
            logPanel.Padding = new Padding(10);
            mainPanel.Controls.Add(logPanel);

            visualizationPanel = new Panel();
            visualizationPanel.Dock = DockStyle.Fill;
            visualizationPanel.BackColor = BackgroundColor;
            visualizationPanel.Padding = new Padding(10);
            mainPanel.Controls.Add(visualizationPanel);
        }

        private void InitializeControls()
        {
            // Array Input
            arrayInputTextBox = new TextBox();
            arrayInputTextBox.Text = "Введите числа через запятую (например: 5,3,8,1,9,2)";
            arrayInputTextBox.ForeColor = Color.Gray;
            arrayInputTextBox.Location = new Point(10, 10);
            arrayInputTextBox.Size = new Size(250, 25);
            arrayInputTextBox.BackColor = ColorTranslator.FromHtml("#1E1E2E");
            arrayInputTextBox.BorderStyle = BorderStyle.FixedSingle;
            arrayInputTextBox.Font = BodyFont;
            
            // Placeholder logic for .NET Framework
            arrayInputTextBox.Enter += (s, e) => {
                if (arrayInputTextBox.Text == "Введите числа через запятую (например: 5,3,8,1,9,2)") {
                    arrayInputTextBox.Text = "";
                    arrayInputTextBox.ForeColor = TextPrimaryColor;
                }
            };
            arrayInputTextBox.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(arrayInputTextBox.Text)) {
                    arrayInputTextBox.Text = "Введите числа через запятую (например: 5,3,8,1,9,2)";
                    arrayInputTextBox.ForeColor = Color.Gray;
                }
            };
            topControlsPanel.Controls.Add(arrayInputTextBox);

            randomArrayButton = new CustomButton("Случайный");
            randomArrayButton.Location = new Point(270, 10);
            randomArrayButton.Click += RandomArrayButton_Click;
            topControlsPanel.Controls.Add(randomArrayButton);

            // Algorithm Selection
            algorithmComboBox = new CustomComboBox();
            algorithmComboBox.Location = new Point(10, 45);
            algorithmComboBox.Size = new Size(150, 25);
            algorithmComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithmComboBox.SelectedIndexChanged += AlgorithmComboBox_SelectedIndexChanged;
            topControlsPanel.Controls.Add(algorithmComboBox);

            speedLabel = new Label();
            speedLabel.Text = "Скорость:";
            speedLabel.ForeColor = TextPrimaryColor;
            speedLabel.Font = BodyFont;
            speedLabel.AutoSize = true;
            speedLabel.Location = new Point(170, 48);
            topControlsPanel.Controls.Add(speedLabel);

            speedTrackBar = new TrackBar();
            speedTrackBar.Minimum = 200;
            speedTrackBar.Maximum = 1500;
            speedTrackBar.Value = 600;
            speedTrackBar.TickFrequency = 100;
            speedTrackBar.SmallChange = 100;
            speedTrackBar.LargeChange = 200;
            speedTrackBar.Width = 150;
            speedTrackBar.Location = new Point(230, 45);
            speedTrackBar.BackColor = PanelColor;
            speedTrackBar.ForeColor = TextPrimaryColor;
            speedTrackBar.TickStyle = TickStyle.None;
            speedTrackBar.ValueChanged += SpeedTrackBar_ValueChanged;
            topControlsPanel.Controls.Add(speedTrackBar);

            // Control Buttons
            startButton = new CustomButton("▶ Старт");
            startButton.Location = new Point(10, 80);
            startButton.Click += StartButton_Click;
            topControlsPanel.Controls.Add(startButton);

            stepButton = new CustomButton("⏭ Шаг");
            stepButton.Location = new Point(120, 80);
            stepButton.Click += StepButton_Click;
            topControlsPanel.Controls.Add(stepButton);

            stopButton = new CustomButton("⏹ Стоп");
            stopButton.Location = new Point(230, 80);
            stopButton.Click += StopButton_Click;
            topControlsPanel.Controls.Add(stopButton);

            resetButton = new CustomButton("↺ Сброс");
            resetButton.Location = new Point(340, 80);
            resetButton.Click += ResetButton_Click;
            topControlsPanel.Controls.Add(resetButton);

            LayoutTopControls();

            // Visualization
            visualizationPictureBox = new PictureBox();
            visualizationPictureBox.Dock = DockStyle.Fill;
            visualizationPictureBox.BackColor = BackgroundColor;
            visualizationPictureBox.Paint += VisualizationPictureBox_Paint;
            visualizationPanel.Controls.Add(visualizationPictureBox);

            // Log
            logListBox = new ListBox();
            logListBox.Dock = DockStyle.Fill;
            logListBox.BackColor = ColorTranslator.FromHtml("#1E1E2E");
            logListBox.ForeColor = TextPrimaryColor;
            logListBox.Font = CodeFont;
            logListBox.BorderStyle = BorderStyle.None;
            logPanel.Controls.Add(logListBox);

            // Hint and Complexity
            hintLabel = new Label();
            hintLabel.Text = "💡 Подсказка:";
            hintLabel.ForeColor = TextPrimaryColor;
            hintLabel.Font = BodyFont;
            hintLabel.Dock = DockStyle.Top;
            hintLabel.AutoSize = false;
            hintLabel.Height = 25;
            hintPanel.Controls.Add(hintLabel);

            complexityLabel = new Label();
            complexityLabel.Text = "Сложность:";
            complexityLabel.ForeColor = TextPrimaryColor;
            complexityLabel.Font = BodyFont;
            complexityLabel.Dock = DockStyle.Left;
            complexityLabel.AutoSize = true;
            hintPanel.Controls.Add(complexityLabel);

            stepsLabel = new Label();
            stepsLabel.Text = "Шагов: 0/0";
            stepsLabel.ForeColor = TextPrimaryColor;
            stepsLabel.Font = BodyFont;
            stepsLabel.Dock = DockStyle.Right;
            stepsLabel.AutoSize = true;
            hintPanel.Controls.Add(stepsLabel);

            // Animation Timer
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = speedTrackBar.Value;
            animationTimer.Tick += AnimationTimer_Tick;

            // Populate algorithm ComboBox
            foreach (var algo in algorithms)
            {
                algorithmComboBox.Items.Add(algo.Name);
            }
            if (algorithmComboBox.Items.Count > 0)
            {
                algorithmComboBox.SelectedIndex = 0;
            }
        }

        private void LayoutTopControls()
        {
            int x = ControlsPadding;
            int y = ControlsPadding;

            int availableWidth = Math.Max(420, topControlsPanel.ClientSize.Width - (ControlsPadding * 2));
            int inputWidth = Math.Min(430, Math.Max(220, availableWidth - randomArrayButton.Width - ControlsSpacing));
            arrayInputTextBox.Location = new Point(x, y);
            arrayInputTextBox.Width = inputWidth;

            randomArrayButton.Location = new Point(arrayInputTextBox.Right + ControlsSpacing, y);

            y = arrayInputTextBox.Bottom + ControlsSpacing;
            algorithmComboBox.Location = new Point(x, y);

            speedLabel.Location = new Point(algorithmComboBox.Right + 15, y + 3);
            speedTrackBar.Location = new Point(speedLabel.Right + 8, y - 3);
            speedTrackBar.Width = Math.Min(220, Math.Max(120, topControlsPanel.ClientSize.Width - speedTrackBar.Left - ControlsPadding));

            y = Math.Max(algorithmComboBox.Bottom, speedTrackBar.Bottom) + ControlsSpacing;
            startButton.Location = new Point(x, y);
            stepButton.Location = new Point(startButton.Right + ControlsSpacing, y);
            stopButton.Location = new Point(stepButton.Right + ControlsSpacing, y);
            resetButton.Location = new Point(stopButton.Right + ControlsSpacing, y);

            int requiredHeight = resetButton.Bottom + ControlsPadding;
            if (topControlsPanel.Height != requiredHeight)
            {
                topControlsPanel.Height = requiredHeight;
            }
        }

        private void RandomArrayButton_Click(object sender, EventArgs e)
        {
            GenerateRandomArray();
        }

        private void AlgorithmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentAlgorithm = algorithms[algorithmComboBox.SelectedIndex];
            UpdateAlgorithmInfo();
            ResetApplication();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (currentAlgorithm == null) return;

            try
            {
                int[] parsedArray = ParseArrayInput();
                if (parsedArray == null || parsedArray.Length == 0)
                {
                    ShowErrorMessage("Массив не может быть пустым.");
                    return;
                }
                initialArray = parsedArray;

                // For search algorithms, prompt for search value
                if (currentAlgorithm is LinearSearch || currentAlgorithm is BinarySearch)
                {
                    string input = Interaction.InputBox("Введите значение для поиска:", "Значение для поиска", "");
                    if (int.TryParse(input, out int searchVal))
                    {
                        searchValue = searchVal;
                        currentAlgorithm.SearchValue = searchValue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(input)) return; // User cancelled
                        ShowErrorMessage("Некорректное значение для поиска.");
                        return;
                    }
                }

                currentTrace = currentAlgorithm.GetTrace(initialArray);
                currentStepIndex = 0;
                isRunning = true;
                animationTimer.Start();
                startButton.Enabled = false;
                stepButton.Enabled = false;
                stopButton.Enabled = true;
                LogStep(currentTrace[currentStepIndex]);
                visualizationPictureBox.Invalidate();
                UpdateStepsCount();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            if (currentAlgorithm == null) return;

            try
            {
                if (currentTrace == null)
                {
                    int[] parsedArray = ParseArrayInput();
                    if (parsedArray == null || parsedArray.Length == 0)
                    {
                        ShowErrorMessage("Массив не может быть пустым.");
                        return;
                    }
                    initialArray = parsedArray;

                    // For search algorithms, prompt for search value
                    if (currentAlgorithm is LinearSearch || currentAlgorithm is BinarySearch)
                    {
                        string input = Interaction.InputBox("Введите значение для поиска:", "Значение для поиска", "");
                        if (int.TryParse(input, out int searchVal))
                        {
                            searchValue = searchVal;
                            currentAlgorithm.SearchValue = searchVal;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(input)) return; // User cancelled
                            ShowErrorMessage("Некорректное значение для поиска.");
                            return;
                        }
                    }

                    currentTrace = currentAlgorithm.GetTrace(initialArray);
                    currentStepIndex = 0;
                }

                if (currentStepIndex < currentTrace.Count)
                {
                    LogStep(currentTrace[currentStepIndex]);
                    visualizationPictureBox.Invalidate();
                    currentStepIndex++;
                    UpdateStepsCount();
                }
                else
                {
                    animationTimer.Stop();
                    isRunning = false;
                    startButton.Enabled = true;
                    stepButton.Enabled = true;
                    stopButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            animationTimer.Stop();
            isRunning = false;
            startButton.Enabled = true;
            stepButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetApplication();
        }

        private void VisualizationPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (currentTrace == null || currentStepIndex == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            int[] arrayToDraw = currentTrace[Math.Min(currentStepIndex - 1, currentTrace.Count - 1)].ArrayState;
            int index1 = currentTrace[Math.Min(currentStepIndex - 1, currentTrace.Count - 1)].Index1;
            int index2 = currentTrace[Math.Min(currentStepIndex - 1, currentTrace.Count - 1)].Index2;
            StepType stepType = currentTrace[Math.Min(currentStepIndex - 1, currentTrace.Count - 1)].Type;

            int barWidth = visualizationPictureBox.Width / (arrayToDraw.Length + 1);
            int maxVal = arrayToDraw.Max();
            int scale = (visualizationPictureBox.Height - 50) / maxVal;

            for (int i = 0; i < arrayToDraw.Length; i++)
            {
                int barHeight = arrayToDraw[i] * scale;
                int x = i * barWidth + (barWidth / 2);
                int y = visualizationPictureBox.Height - barHeight - 20;

                Color barColor = BarNormalColor;
                if (i == index1 || i == index2)
                {
                    if (stepType == StepType.Compare) barColor = Accent1Color;
                    else if (stepType == StepType.Swap) barColor = Accent2Color;
                    else if (stepType == StepType.Found) barColor = BarFoundColor;
                }

                using (SolidBrush brush = new SolidBrush(barColor))
                {
                    g.FillRectangle(brush, x, y, barWidth - 5, barHeight);
                }

                // Draw value on top of bar
                using (SolidBrush textBrush = new SolidBrush(TextPrimaryColor))
                {
                    SizeF stringSize = g.MeasureString(arrayToDraw[i].ToString(), BarLabelFont);
                    g.DrawString(arrayToDraw[i].ToString(), BarLabelFont, textBrush, x + (barWidth - 5) / 2 - stringSize.Width / 2, y - stringSize.Height);
                }
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            StepButton_Click(sender, e);
        }

        private int[] ParseArrayInput()
        {
            string input = arrayInputTextBox.Text;
            if (string.IsNullOrWhiteSpace(input))
            {
                return new int[0];
            }

            try
            {
                int[] array = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(s => int.Parse(s.Trim()))
                                   .ToArray();

                if (array.Any(x => x < 1 || x > 999))
                {
                    throw new ArgumentOutOfRangeException("Числа в массиве должны быть в диапазоне от 1 до 999.");
                }
                return array;
            }
            catch (FormatException)
            {
                throw new FormatException("Некорректный формат ввода. Пожалуйста, введите числа, разделенные запятыми.");
            }
            catch (OverflowException)
            {
                throw new OverflowException("Одно или несколько чисел слишком велики или малы.");
            }
        }

        private void GenerateRandomArray()
        {
            Random rand = new Random();
            int size = rand.Next(8, 16); // 8-15 elements
            int[] randomArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                randomArray[i] = rand.Next(1, 100); // Numbers between 1 and 99
            }
            arrayInputTextBox.Text = string.Join(", ", randomArray);
            ResetApplication();
        }

        private void InitializeAlgorithms()
        {
            algorithms = new List<Algorithm>
            {
                new BubbleSort(),
                new SelectionSort(),
                new InsertionSort(),
                new LinearSearch(),
                new BinarySearch()
            };
        }

        private void ResetApplication()
        {
            animationTimer?.Stop();
            isRunning = false;
            currentTrace = null;
            currentStepIndex = 0;
            searchValue = 0;

            if (algorithmComboBox != null && algorithmComboBox.SelectedIndex >= 0 && algorithms != null)
            {
                currentAlgorithm = algorithms[algorithmComboBox.SelectedIndex];
                UpdateAlgorithmInfo();
            }

            logListBox?.Items.Clear();
            stepsLabel.Text = "Шагов: 0/0";
            startButton.Enabled = true;
            stepButton.Enabled = true;
            stopButton.Enabled = false;
            visualizationPictureBox?.Invalidate();
        }

        private void SpeedTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (animationTimer != null)
            {
                animationTimer.Interval = speedTrackBar.Value;
            }
        }

        private void LogStep(TraceStep step)
        {
            logListBox.Items.Add(step.Description);
            logListBox.TopIndex = logListBox.Items.Count - 1; // Scroll to bottom
            hintLabel.Text = "💡 Подсказка: " + currentAlgorithm.Hint;
        }

        private void UpdateAlgorithmInfo()
        {
            if (currentAlgorithm != null)
            {
                complexityLabel.Text = "Сложность: " + currentAlgorithm.Complexity;
                hintLabel.Text = "💡 Подсказка: " + currentAlgorithm.Hint;
            }
        }

        private void UpdateStepsCount()
        {
            if (currentTrace != null)
            {
                stepsLabel.Text = $"Шагов: {currentStepIndex}/{currentTrace.Count}";
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog();
            }
        }

        private void TitleBarPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "AlgoRythm Desktop";
            this.ResumeLayout(false);

        }
    }
}
