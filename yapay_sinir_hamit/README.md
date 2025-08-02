# YSA (Yapay Sinir Ağı) ile Harf Tanıma

Bu proje, A, B, C, D, E harflerini tanıyan bir YSA (Çok Katmanlı Algılayıcı / MLP) uygulamasıdır. Proje, Windows Forms kullanılarak C# ile geliştirilmiştir.

## Proje Yapısı

Projede aşağıdaki sınıflar yer almaktadır:

- **Neuron**: Sinir ağındaki bir nöronu temsil eder.
- **Layer**: Nöronlardan oluşan bir katmanı temsil eder.
- **NeuralNetwork**: Çok Katmanlı Algılayıcı (MLP) yapay sinir ağını uygular.
- **DataProvider**: Eğitim veri setini sağlar.
- **Form1**: Kullanıcı arayüzünü oluşturur ve neural network ile etkileşimi sağlar.

## Mimari

YSA mimarisi şu şekildedir:

- **Giriş Katmanı**: 35 nöron (7x5 piksel matrisi)
- **Gizli Katman**: 15 nöron
- **Çıkış Katmanı**: 5 nöron (A, B, C, D, E harfleri)

## Nasıl Kullanılır

1. Uygulamayı başlatın.
2. "Eğit" butonuna basarak sinir ağını eğitin.
3. Solda bulunan 7x5 matris üzerinde tanımak istediğiniz harfi çizin.
4. "Tanımla" butonuna basarak sonucu görün.

## Özellikler

- Hata oranını ayarlayabilme (epsilon değeri)
- Çizim matrisini temizleme
- Her harf için çıkış değerlerini görüntüleme
- Tanıma sonucunu gösterme

## Uygulama Detayları

### Neuron (Nöron) Sınıfı

Her nöronun ağırlıkları, bias değeri ve çıkış değerini tutar. Sigmoid aktivasyon fonksiyonunu kullanır.

### Layer (Katman) Sınıfı

Bir katmanda yer alan nöronları yönetir ve katmanın çıkışlarını hesaplar.

### NeuralNetwork (Sinir Ağı) Sınıfı

Giriş katmanı, gizli katman ve çıkış katmanından oluşan sinir ağını uygular. İleri besleme (forward propagation) ve geriye yayılım (backpropagation) algoritmalarını içerir.

### DataProvider (Veri Sağlayıcı) Sınıfı

A, B, C, D, E harflerini temsil eden 7x5 piksel matrislerini ve beklenen çıkış değerlerini sağlar.

### Form1 (Ana Form) Sınıfı

Kullanıcı arayüzünü oluşturur, çizim matrisini yönetir ve sinir ağı ile etkileşimi sağlar.

## Algoritma

1. Sinir ağı, rasgele ağırlık değerleri ile başlatılır.
2. Eğitim verileri kullanılarak ileri besleme algoritması çalıştırılır.
3. Gerçek çıkış ile beklenen çıkış arasındaki hata hesaplanır.
4. Geriye yayılım algoritması ile ağırlıklar güncellenir.
5. Hata belirli bir eşik değerinin (epsilon) altına düşene kadar 2-4 adımları tekrarlanır.
6. Eğitim tamamlandıktan sonra, kullanıcının çizdiği harf tanınır.

## Notlar

- Eğitim tamamlandıktan sonra "Tanımla" butonuna basılmalıdır.
- Çizilen harf tam olarak tanınmazsa, daha net bir şekilde tekrar çizmeyi deneyin.
- Hata oranını düşürerek daha hassas bir model elde edebilirsiniz (ancak eğitim süresi artar). 