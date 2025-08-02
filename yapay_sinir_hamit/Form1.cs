using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yapay_sinir_hamit
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Yapay sinir ağı
        /// </summary>
        private NeuralNetwork neuralNetwork;

        /// <summary>
        /// Kullanıcının çizdiği harf için matris
        /// </summary>
        private bool[,] drawingMatrix;

        /// <summary>
        /// Çizim için kullanılacak butonlar
        /// </summary>
        private Button[,] gridButtons;

        /// <summary>
        /// Eğitim tamamlandı mı?
        /// </summary>
        private bool isNetworkTrained = false;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        /// <summary>
        /// Form bileşenlerini başlatır ve yerleştirir
        /// </summary>
        private void InitializeCustomComponents()
        {
            // Form boyutunu ayarla
            this.Width = 800;
            this.Height = 600;
            this.Text = "YSA (Backpropagation) Harf Tanıma";

            // 7x5'lik çizim matrisi
            drawingMatrix = new bool[7, 5];
            gridButtons = new Button[7, 5];

            // Çizim alanını oluştur (sol panel)
            Panel drawingPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(250, 350),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.Black
            };
            this.Controls.Add(drawingPanel);

            // Çizim için butonları yerleştir
            const int buttonSize = 40;
            const int margin = 5;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Button button = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * (buttonSize + margin) + margin, row * (buttonSize + margin) + margin),
                        Tag = new Point(row, col),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat
                    };

                    button.Click += GridButton_Click;
                    gridButtons[row, col] = button;
                    drawingPanel.Controls.Add(button);
                }
            }

            // Temizle butonu
            Button clearButton = new Button
            {
                Location = new Point(20, 380),
                Size = new Size(120, 40),
                Text = "Temizle",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            clearButton.Click += ClearButton_Click;
            this.Controls.Add(clearButton);

            // Çizgileri kaldır butonu
            Button removeDrawingsButton = new Button
            {
                Location = new Point(20, 430),
                Size = new Size(120, 40),
                Text = "Çizgileri Kaldır",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            removeDrawingsButton.Click += RemoveDrawingsButton_Click;
            this.Controls.Add(removeDrawingsButton);

            // Sonuç paneli (sağ panel)
            GroupBox resultsPanel = new GroupBox
            {
                Location = new Point(290, 20),
                Size = new Size(480, 350),
                Text = "Sonuçlar",
                Name = "Sonuçlar"
            };
            this.Controls.Add(resultsPanel);

            // A çıktısı
            Label lblOutputA = new Label
            {
                Location = new Point(20, 40),
                Size = new Size(440, 25),
                Text = "A çıkışı = -",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblOutputA);

            // B çıktısı
            Label lblOutputB = new Label
            {
                Location = new Point(20, 70),
                Size = new Size(440, 25),
                Text = "B çıkışı = -",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblOutputB);

            // C çıktısı
            Label lblOutputC = new Label
            {
                Location = new Point(20, 100),
                Size = new Size(440, 25),
                Text = "C çıkışı = -",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblOutputC);

            // D çıktısı
            Label lblOutputD = new Label
            {
                Location = new Point(20, 130),
                Size = new Size(440, 25),
                Text = "D çıkışı = -",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblOutputD);

            // E çıktısı
            Label lblOutputE = new Label
            {
                Location = new Point(20, 160),
                Size = new Size(440, 25),
                Text = "E çıkışı = -",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblOutputE);

            // Tanıma sonucu
            Label lblRecognitionResult = new Label
            {
                Location = new Point(20, 210),
                Size = new Size(440, 40),
                Text = "Tanıma = -",
                Font = new Font("Arial", 14, FontStyle.Bold)
            };
            resultsPanel.Controls.Add(lblRecognitionResult);

            // Hata oranı etiketi
            Label lblErrorRate = new Label
            {
                Location = new Point(20, 300),
                Size = new Size(150, 25),
                Text = "Hata oranı:",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(lblErrorRate);

            // Hata oranı giriş kutusu
            TextBox txtErrorRate = new TextBox
            {
                Location = new Point(180, 300),
                Size = new Size(100, 25),
                Text = "0.01",
                Font = new Font("Arial", 10)
            };
            resultsPanel.Controls.Add(txtErrorRate);

            // Eğit butonu
            Button trainButton = new Button
            {
                Location = new Point(290, 380),
                Size = new Size(150, 40),
                Text = "Eğit",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            trainButton.Click += TrainButton_Click;
            this.Controls.Add(trainButton);

            // Test butonu
            Button testButton = new Button
            {
                Location = new Point(450, 380),
                Size = new Size(150, 40),
                Text = "Tanımla",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            testButton.Click += TestButton_Click;
            this.Controls.Add(testButton);

            // Sinir ağını başlat (varsayılan değerlerle)
            InitializeNeuralNetwork();
        }

        /// <summary>
        /// Sinir ağını oluşturur ve başlatır
        /// </summary>
        private void InitializeNeuralNetwork()
        {
            // 35 giriş (7x5 matrix), 15 gizli nöron, 5 çıkış (A, B, C, D, E)
            neuralNetwork = new NeuralNetwork(35, 15, 5, 0.1);
        }

        /// <summary>
        /// Çizim matrisindeki bir butona tıklandığında çağrılır
        /// </summary>
        private void GridButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Point position = (Point)clickedButton.Tag;
            int row = position.X;
            int col = position.Y;

            // Butonun durumunu değiştir
            drawingMatrix[row, col] = !drawingMatrix[row, col];
            clickedButton.BackColor = drawingMatrix[row, col] ? Color.Black : Color.White;
        }

        /// <summary>
        /// Temizle butonuna tıklandığında çağrılır
        /// </summary>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearDrawingMatrix();
        }

        /// <summary>
        /// Çizgileri kaldır butonuna tıklandığında çağrılır
        /// </summary>
        private void RemoveDrawingsButton_Click(object sender, EventArgs e)
        {
            ClearDrawingMatrix();
        }

        /// <summary>
        /// Çizim matrisini temizler
        /// </summary>
        private void ClearDrawingMatrix()
        {
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    drawingMatrix[row, col] = false;
                    gridButtons[row, col].BackColor = Color.White;
                }
            }

            // Çıkış etiketlerini temizle
            GroupBox? resultsPanel = this.Controls.Find("Sonuçlar", false).FirstOrDefault() as GroupBox;
            if (resultsPanel != null)
            {
                foreach (Control control in resultsPanel.Controls)
                {
                    if (control is Label label && label.Text.Contains("çıkışı"))
                    {
                        label.Text = label.Text.Split('=')[0] + "= -";
                    }
                    else if (control is Label recognitionLabel && recognitionLabel.Text.Contains("Tanıma"))
                    {
                        recognitionLabel.Text = "Tanıma = -";
                    }
                }
            }
        }

        /// <summary>
        /// Eğit butonuna tıklandığında çağrılır
        /// </summary>
        private void TrainButton_Click(object sender, EventArgs e)
        {
            // Hata oranını al
            GroupBox? resultsPanel = this.Controls.Find("Sonuçlar", false).FirstOrDefault() as GroupBox;
            TextBox? txtErrorRate = null;
            double epsilon = 0.01; // Varsayılan değer

            if (resultsPanel != null)
            {
                foreach (Control control in resultsPanel.Controls)
                {
                    if (control is TextBox textBox)
                    {
                        txtErrorRate = textBox;
                        break;
                    }
                }
            }

            if (txtErrorRate != null && !string.IsNullOrEmpty(txtErrorRate.Text))
            {
                if (double.TryParse(txtErrorRate.Text, out double parsedEpsilon))
                {
                    epsilon = parsedEpsilon;
                }
            }

            // Eğitim verilerini al
            double[][] trainingInputs = DataProvider.GetTrainingInputs();
            double[][] trainingOutputs = DataProvider.GetTrainingOutputs();

            // Ağı eğit
            double error = neuralNetwork.Train(trainingInputs, trainingOutputs, 100000, epsilon);
            isNetworkTrained = true;

            MessageBox.Show($"Eğitim tamamlandı! Son hata: {error}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Test butonuna tıklandığında çağrılır
        /// </summary>
        private void TestButton_Click(object sender, EventArgs e)
        {
            if (!isNetworkTrained)
            {
                MessageBox.Show("Lütfen önce ağı eğitin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Çizilen harfi giriş vektörüne dönüştür
            double[] testInput = DataProvider.CreateInputFromMatrix(drawingMatrix);

            // Ağı test et
            double[] outputs = neuralNetwork.FeedForward(testInput);
            int prediction = neuralNetwork.Predict(testInput);
            string recognizedLetter = DataProvider.GetLetterFromPrediction(prediction);

            // Sonuçları göster
            GroupBox? resultsPanel = this.Controls.Find("Sonuçlar", false).FirstOrDefault() as GroupBox;
            if (resultsPanel != null)
            {
                foreach (Control control in resultsPanel.Controls)
                {
                    if (control is Label label)
                    {
                        if (label.Text.Contains("A çıkışı"))
                        {
                            label.Text = $"A çıkışı = {outputs[0]}";
                        }
                        else if (label.Text.Contains("B çıkışı"))
                        {
                            label.Text = $"B çıkışı = {outputs[1]}";
                        }
                        else if (label.Text.Contains("C çıkışı"))
                        {
                            label.Text = $"C çıkışı = {outputs[2]}";
                        }
                        else if (label.Text.Contains("D çıkışı"))
                        {
                            label.Text = $"D çıkışı = {outputs[3]}";
                        }
                        else if (label.Text.Contains("E çıkışı"))
                        {
                            label.Text = $"E çıkışı = {outputs[4]}";
                        }
                        else if (label.Text.Contains("Tanıma"))
                        {
                            label.Text = $"Tanıma = {recognizedLetter}";
                        }
                    }
                }
            }
        }
    }
}
