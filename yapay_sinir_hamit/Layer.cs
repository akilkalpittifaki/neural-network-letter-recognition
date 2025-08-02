using System;
using System.Collections.Generic;

// Layer.cs
// Layer sınıfı: Nöronlardan oluşan bir katmanı temsil eder
// Layer(int neuronCount, int inputsPerNeuron): Belirli sayıda nöron içeren katman oluşturur
// CalculateOutputs(double[] inputs): Katmandaki tüm nöronlar için çıkış hesaplar

namespace yapay_sinir_hamit
{
    /// <summary>
    /// Yapay sinir ağındaki bir katmanı temsil eden sınıf.
    /// Her katman, belirli sayıda nörondan oluşur.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Katmandaki nöronlar
        /// </summary>
        public Neuron[] Neurons { get; private set; }

        /// <summary>
        /// Katmandaki nöron sayısı
        /// </summary>
        public int NeuronCount => Neurons.Length;

        /// <summary>
        /// Katmanın çıkış değerleri
        /// </summary>
        public double[] Outputs { get; private set; }

        /// <summary>
        /// Yeni bir katman oluşturur
        /// </summary>
        /// <param name="neuronCount">Katmandaki nöron sayısı</param>
        /// <param name="inputsPerNeuron">Her nörona gelen bağlantı sayısı</param>
        public Layer(int neuronCount, int inputsPerNeuron)
        {
            Neurons = new Neuron[neuronCount];
            Outputs = new double[neuronCount];

            // Katmandaki tüm nöronları oluştur
            for (int i = 0; i < neuronCount; i++)
            {
                Neurons[i] = new Neuron(inputsPerNeuron);
            }
        }

        /// <summary>
        /// Katmanın çıkışlarını hesaplar
        /// </summary>
        /// <param name="inputs">Katmana giren giriş değerleri</param>
        /// <returns>Katmanın çıkış değerleri</returns>
        public double[] CalculateOutputs(double[] inputs)
        {
            for (int i = 0; i < NeuronCount; i++)
            {
                Outputs[i] = Neurons[i].CalculateOutput(inputs);
            }
            return Outputs;
        }
    }
} 

// Layer (Katman) Sınıfındaki Giriş ve Çıkışları Anlama
// Layer.cs dosyasındaki giriş ve çıkışlar, yapay sinir ağında veri akışının nasıl ilerlediğini gösterir:
// Girişler (inputs)
// Ne Giriyor? CalculateOutputs(double[] inputs) fonksiyonundaki inputs parametresi, bu katmana gelen giriş verileridir.
// Örnek:
// Gizli katman için: 7x5=35 elemanlı harf pikselleri (0 veya 1 değerleri)
// Çıkış katmanı için: Gizli katmandan gelen 15 elemanlı çıkış değerleri
// Çıkışlar (outputs)
// Ne Çıkıyor? Outputs dizisi, katmandaki her nöronun hesapladığı çıkış değerlerini tutar.
// Örnek:
// Gizli katmandan: 15 elemanlı dizi (gizli katmanınızdaki nöron sayısı)
// Çıkış katmanından: 5 elemanlı dizi (A-B-C-D-E harfleri için olasılık değerleri)
// Somut Örnek
// Harf "A" için veri akışı şöyle olur:
// Giriş Katmanı:
// 7x5 matristeki "A" harfi pikselleri düzleştirilir (35 eleman)
// Apply to Layer.cs
// Gizli Katman:
// CalculateOutputs fonksiyonu 35 elemanlı giriş alır
// Her nöron bu girişleri ağırlıklarıyla işler
// Çıkış olarak 15 elemanlı dizi üretir (0-1 arası değerler)
// Apply to Layer.cs
// Çıkış Katmanı:
// CalculateOutputs fonksiyonu 15 elemanlı giriş alır (gizli katmanın çıkışı)
// 5 elemanlı çıkış üretir (her harf için bir değer)
// Apply to Layer.cs
// Layer sınıfı, bu veri akışını sağlamak için katmandaki her nöronun hesaplamasını yönetir ve sonuçları toplar

