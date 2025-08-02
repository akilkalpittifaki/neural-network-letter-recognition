using System;

namespace yapay_sinir_hamit
{
    /// <summary>
    /// Eğitim ve test verilerini sağlayan sınıf.
    /// A, B, C, D, E harflerinin 7x5'lik matrislerini ve beklenen çıkışları sağlar.
    /// </summary>
    public static class DataProvider
    {
        /// <summary>
        /// A, B, C, D, E harflerinin piksel verilerini içeren 3 boyutlu dizi
        /// [harf indeksi, satır, sütun]
        /// </summary>
        private static readonly int[,,] letterData = new int[5, 7, 5]
        {
            // A harfi
            {
                { 0, 0, 1, 0, 0 },
                { 0, 1, 0, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 }
            },
            // B harfi
            {
                { 1, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 0 }
            },
            // C harfi
            {
                { 0, 0, 1, 1, 1 },
                { 0, 1, 0, 0, 0 },
                { 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1 }
            },
            // D harfi
            {
                { 1, 1, 1, 0, 0 },
                { 1, 0, 0, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 0, 1 },
                { 1, 0, 0, 1, 0 },
                { 1, 1, 1, 0, 0 }
            },
            // E harfi
            {
                { 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 0 },
                { 1, 0, 0, 0, 0 },
                { 1, 1, 1, 1, 1 }
            }
        };

        /// <summary>
        /// Beklenen çıkış değerleri (one-hot encoding)
        /// Örneğin, A harfi için [1,0,0,0,0], B harfi için [0,1,0,0,0], vb.
        /// </summary>
        private static readonly double[][] expectedOutputs = new double[][]
        {
            new double[] { 1, 0, 0, 0, 0 }, // A harfi
            new double[] { 0, 1, 0, 0, 0 }, // B harfi
            new double[] { 0, 0, 1, 0, 0 }, // C harfi
            new double[] { 0, 0, 0, 1, 0 }, // D harfi
            new double[] { 0, 0, 0, 0, 1 }  // E harfi
        };

        /// <summary>
        /// Giriş verilerini (7x5 matris) düzleştirilmiş bir diziye dönüştürür
        /// </summary>
        /// <param name="letterIndex">Harf indeksi (0-4 arası)</param>
        /// <returns>Düzleştirilmiş 35 elemanlı dizi (7x5)</returns>
        public static double[] GetFlattenedInput(int letterIndex)
        {
            double[] result = new double[35]; // 7x5 = 35
            int index = 0;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    result[index++] = letterData[letterIndex, row, col];
                }
            }

            return result;
        }

        /// <summary>
        /// Özel bir giriş matrisi oluşturur (kullanıcının çizdiği harf için)
        /// </summary>
        /// <param name="matrix">7x5'lik boolean matris</param>
        /// <returns>Düzleştirilmiş 35 elemanlı dizi</returns>
        public static double[] CreateInputFromMatrix(bool[,] matrix)
        {
            double[] result = new double[35];
            int index = 0;

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    result[index++] = matrix[row, col] ? 1.0 : 0.0;
                }
            }

            return result;
        }

        /// <summary>
        /// Eğitim giriş verilerini döndürür
        /// </summary>
        /// <returns>Eğitim giriş verileri</returns>
        public static double[][] GetTrainingInputs()
        {
            double[][] trainingInputs = new double[5][];
            
            for (int i = 0; i < 5; i++)
            {
                trainingInputs[i] = GetFlattenedInput(i);
            }

            return trainingInputs;
        }

        /// <summary>
        /// Eğitim çıkış (hedef) verilerini döndürür
        /// </summary>
        /// <returns>Eğitim çıkış verileri</returns>
        public static double[][] GetTrainingOutputs()
        {
            return expectedOutputs;
        }

        /// <summary>
        /// Tahmin sonucunu harfe dönüştürür
        /// </summary>
        /// <param name="prediction">Tahmin indeksi (0-4 arası)</param>
        /// <returns>Harf (A, B, C, D, E)</returns>
        public static string GetLetterFromPrediction(int prediction)
        {
            switch (prediction)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "C";
                case 3: return "D";
                case 4: return "E";
                default: return "?";
            }
        }
    }
} 