using System;
using System.Collections.Generic;
using System.Linq;

// NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learningRate): 
//3 katmanlı YSA oluşturur

namespace yapay_sinir_hamit
{
    /// <summary>
    /// Çok Katmanlı Algılayıcı (MLP) yapay sinir ağını temsil eden sınıf.
    /// Giriş katmanı, bir gizli katman ve çıkış katmanından oluşur.
    /// Geriye yayılım algoritması ile eğitilir.
    /// </summary>
    public class NeuralNetwork
    {
        /// <summary>
        /// Ağın katmanları (gizli ve çıkış katmanları)
        /// </summary>
        private Layer[] layers;

        /// <summary>
        /// Giriş katmanının boyutu (nöron sayısı)
        /// </summary>
        public int InputSize { get; private set; }

        /// <summary>
        /// Gizli katmanın boyutu (nöron sayısı)
        /// </summary>
        public int HiddenSize { get; private set; } ///hidden gizli 

        /// <summary>
        /// Çıkış katmanının boyutu (nöron sayısı)
        /// </summary>
        public int OutputSize { get; private set; }

        /// <summary>
        /// Eğitim için öğrenme oranı (learning rate)
        /// </summary>
        private double learningRate;

        /// <summary>
        /// Son hata değeri
        /// </summary>
        public double Error { get; private set; }

        /// <summary>
        /// Yeni bir MLP sinir ağı oluşturur
        /// </summary>
        /// <param name="inputSize">Giriş katmanının boyutu</param>
        /// <param name="hiddenSize">Gizli katmanın boyutu</param>
        /// <param name="outputSize">Çıkış katmanının boyutu</param>
        /// <param name="learningRate">Öğrenme oranı</param>
        ///NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learningRate): 3 katmanlı YSA oluşturur
        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize, double learningRate = 0.1)
        {
            InputSize = inputSize;
            HiddenSize = hiddenSize;
            OutputSize = outputSize;
            this.learningRate = learningRate;

            // Gizli ve çıkış katmanlarını oluştur
            layers = new Layer[2];
            layers[0] = new Layer(hiddenSize, inputSize); // Gizli katman
            layers[1] = new Layer(outputSize, hiddenSize); // Çıkış katmanı
        }

        /// <summary>
        /// İleri besleme (forward propagation) işlemi
        /// </summary>
        /// <param name="inputs">Giriş değerleri</param>
        /// <returns>Çıkış değerleri</returns>
        public double[] FeedForward(double[] inputs)
        {
            // Giriş kontrolü
            if (inputs.Length != InputSize)
                throw new ArgumentException($"Giriş vektörü {InputSize} elemanlı olmalıdır, ancak {inputs.Length} eleman var.");

            // Gizli katmanın çıkışlarını hesapla
            double[] hiddenOutputs = layers[0].CalculateOutputs(inputs);

            // Çıkış katmanının çıkışlarını hesapla ve döndür
            return layers[1].CalculateOutputs(hiddenOutputs);
        }

        /// <summary>
        /// Geriye yayılım (backpropagation) algoritması ile ağırlıkları günceller
        /// </summary>
        /// <param name="inputs">Giriş değerleri</param>
        /// <param name="targets">Hedef çıkış değerleri</param>
        /// <returns>Hata miktarı</returns>
        public double Backpropagate(double[] inputs, double[] targets)
        {
            // İleri besleme ile çıkışları hesapla
            double[] outputs = FeedForward(inputs);

            // Çıkış katmanı hatasını hesapla
            for (int i = 0; i < layers[1].NeuronCount; i++)
            {
                double error = targets[i] - outputs[i];
                layers[1].Neurons[i].Delta = error * layers[1].Neurons[i].SigmoidDerivative(outputs[i]);
            }

            // Gizli katman hatasını hesapla
            for (int i = 0; i < layers[0].NeuronCount; i++)
            {
                double error = 0;
                for (int j = 0; j < layers[1].NeuronCount; j++)
                {
                    error += layers[1].Neurons[j].Delta * layers[1].Neurons[j].Weights[i];
                }
                layers[0].Neurons[i].Delta = error * layers[0].Neurons[i].SigmoidDerivative(layers[0].Outputs[i]);
            }

            // Çıkış katmanı ağırlıklarını güncelle
            for (int i = 0; i < layers[1].NeuronCount; i++)
            {
                for (int j = 0; j < layers[1].Neurons[i].Weights.Length; j++)
                {
                    layers[1].Neurons[i].Weights[j] += learningRate * layers[1].Neurons[i].Delta * layers[0].Outputs[j];
                }
                layers[1].Neurons[i].Bias += learningRate * layers[1].Neurons[i].Delta;
            }

            // Gizli katman ağırlıklarını güncelle
            for (int i = 0; i < layers[0].NeuronCount; i++)
            {
                for (int j = 0; j < layers[0].Neurons[i].Weights.Length; j++)
                {
                    layers[0].Neurons[i].Weights[j] += learningRate * layers[0].Neurons[i].Delta * inputs[j];
                }
                layers[0].Neurons[i].Bias += learningRate * layers[0].Neurons[i].Delta;
            }

            // Toplam hata karelerini hesapla
            double squaredError = 0;
            for (int i = 0; i < outputs.Length; i++)
            {
                double error = targets[i] - outputs[i];
                squaredError += error * error;
            }
            Error = squaredError / outputs.Length;

            return Error;
        }

        /// <summary>
        /// Ağı eğitir
        /// </summary>
        /// <param name="trainingInputs">Eğitim giriş verileri</param>
        /// <param name="trainingOutputs">Eğitim çıkış verileri</param>
        /// <param name="epochs">Yineleme sayısı</param>
        /// <param name="epsilon">Kabul edilebilir hata eşiği</param>
        /// <returns>Son iterasyondaki hata değeri</returns>
        public double Train(double[][] trainingInputs, double[][] trainingOutputs, int epochs, double epsilon)
        {
            double currentError = double.MaxValue;
            int epoch = 0;

            while (epoch < epochs && currentError > epsilon)
            {
                currentError = 0;
                for (int i = 0; i < trainingInputs.Length; i++)
                {
                    currentError += Backpropagate(trainingInputs[i], trainingOutputs[i]);
                }
                currentError /= trainingInputs.Length; // Ortalama hata
                epoch++;
            }

            return currentError;
        }

        /// <summary>
        /// Ağın çıktısı üzerinden tahmin yapar (en yüksek değerli sınıfı seçer)
        /// </summary>
        /// <param name="input">Giriş verisi</param>
        /// <returns>Tahmin edilen sınıf indeksi (0-4 arası)</returns>
        public int Predict(double[] input) ///Predict(double[] input): En yüksek çıkış değerine sahip sınıfı bulur
        {
            double[] outputs = FeedForward(input);
            return Array.IndexOf(outputs, outputs.Max());
        }

        /// <summary>
        /// Gizli ve çıkış katmanlarındaki tüm ağırlıkları ve bias değerlerini döndürür
        /// </summary>
        /// <returns>Ağırlık ve bias değerleri listesi</returns>
        public List<double> GetWeightsAndBiases()
        {
            List<double> weightsAndBiases = new List<double>();

            foreach (var layer in layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    weightsAndBiases.AddRange(neuron.Weights);
                    weightsAndBiases.Add(neuron.Bias);
                }
            }

            return weightsAndBiases;
        }
    }
} 

// Yapay Sinir Ağı Eğitim Süreci Nasıl Çalışıyor?
// Train metodu, yapay sinir ağının A, B, C, D, E harflerini tanımayı öğrenmesini sağlayan fonksiyondur. Eğitim süreci şöyle gerçekleşir:
// 1. Eğitim Verileri
// trainingInputs: 5 harf için 5 adet 35 elemanlı giriş verisi (7x5 matris)
// trainingOutputs: 5 harf için beklenen çıkışlar ([1,0,0,0,0], [0,1,0,0,0], vb.)
// 2. Eğitim Döngüsü
// Apply to NeuralNetwor...
// 3. Adım Adım Eğitim Süreci:
// Başlangıç: Bütün ağırlıklar rastgele değerlerle başlar (-0.5 ile 0.5 arası)
// Her Harf İçin Tekrarla:
// A harfinin giriş verileri ağa verilir
// Ağ bir tahmin üretir (başlangıçta rastgele çıkışlar)
// Bu tahmin beklenen çıkışla karşılaştırılır ([1,0,0,0,0])
// Hata hesaplanır (beklenen - gerçek)
// Backpropagate ile ağırlıklar güncellenir:
// Apply to NeuralNetwor...
// Tekrarlama:
// Bu süreç her harf için tekrarlanır
// Sonra baştan başlayarak yeni bir "epoch" (döngü) başlar
// Hata değeri epsilon değerinden küçük olunca veya maksimum epoch sayısına ulaşınca eğitim biter
// 4. Öğrenme Oranı (Learning Rate)
// learningRate (0.1 değeri): Her adımda ağırlıkların ne kadar değişeceğini belirler
// Düşük değer: Yavaş öğrenme ama daha istikrarlı
// Yüksek değer: Hızlı öğrenme ama dengesiz olabilir
// 5. Epsilon ve Epochs
// epsilon: Kabul edilebilir hata eşiği (örn: 0.01)
// epochs: Maksimum yineleme sayısı (100,000)
// Eğitim bu iki koşuldan biri sağlanınca durur
// Sonuç olarak, eğitim süreci ağın ağırlıklarını sürekli ayarlayarak hata değerini azaltmaya çalışır ve zamanla ağ doğru harfi tanımayı öğrenir.

