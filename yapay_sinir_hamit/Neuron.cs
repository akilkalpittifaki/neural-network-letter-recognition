using System;

namespace yapay_sinir_hamit
{
    /// <summary>
    /// Yapay sinir ağında bir nöronu temsil eden sınıf.
    /// Her nöron, giriş değerlerini ağırlıklarla çarpıp bir aktivasyon fonksiyonundan geçirerek çıkış üretir.
    /// </summary>
    public class Neuron
    {
        /// <summary>
        /// Nöronun ağırlık değerleri
        /// </summary>
        public double[] Weights { get; set; }

        /// <summary>
        /// Nöronun bias değeri (eşik değeri)
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        /// Nöronun çıkış değeri
        /// </summary>
        public double Output { get; set; }

        /// <summary>
        /// Geriye yayılım algoritması için hata değeri
        /// </summary>
        public double Delta { get; set; }

        /// <summary>
        /// Rasgele sayı üreteci
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Nöron oluşturur ve ağırlıkları rasgele başlatır
        /// </summary>
        /// <param name="inputCount">Nörona bağlanan giriş sayısı</param>
        public Neuron(int inputCount)
        {
            Weights = new double[inputCount];
            // Ağırlıkları -0.5 ile 0.5 arasında rasgele değerlerle başlat
            for (int i = 0; i < inputCount; i++)
            {
                Weights[i] = random.NextDouble() - 0.5;
            }
            // Bias değerini de -0.5 ile 0.5 arasında rasgele başlat
            Bias = random.NextDouble() - 0.5;
        }

        /// <summary>
        /// Nöronun çıkış değerini hesaplar
        /// </summary>
        /// <param name="inputs">Giriş değerleri</param>
        /// <returns>Hesaplanan çıkış değeri</returns>
        public double CalculateOutput(double[] inputs)
        {
            double sum = Bias;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            Output = Sigmoid(sum);
            return Output;
        }

        /// <summary>
        /// Sigmoid aktivasyon fonksiyonu
        /// </summary>
        /// <param name="x">Giriş değeri</param>
        /// <returns>Sigmoid fonksiyonunun sonucu (0-1 arasında)</returns>
        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        /// <summary>
        /// Sigmoid fonksiyonunun türevi
        /// </summary>
        /// <param name="x">Türevi alınacak değer</param>
        /// <returns>Türev değeri</returns>
        ///SigmoidDerivative(double x): Öğrenme sürecinde kullanılan türev fonksiyonu
        public double SigmoidDerivative(double x)
        {
            return x * (1 - x);
        }
    }
} 