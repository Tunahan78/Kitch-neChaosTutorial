# Kitchen Chaos - Unity Oyun Geliştirme Projesi

<div align="center">
  <video src="https://github.com/user-attachments/assets/5aed4b9f-9d44-4586-8e79-35ad5c1f15ce.mp4
 width="100%" />
  

</div>



<br>

Bu proje, popüler "Overcooked" tarzı bir oyun mekaniğini yeniden oluşturarak, modern oyun geliştirme mimarilerini ve temiz kod prensiplerini uygulamaya odaklanan bir eğitim projesidir. Projenin temel amacı, sadece bir oyun klonu oluşturmak değil, aynı zamanda esnek, genişletilebilir ve bakımı kolay, profesyonel standartlarda bir sistem kurmaktır.

---

## Mimarinin Temelleri ve Teknik Kararlar

Bu projede, kodun kalitesini ve yönetilebilirliğini artırmak için endüstri standardı kabul edilen yazılım prensipleri ve tasarım desenleri temel alınmıştır.

### 🏗️ SOLID Prensipleri

- **Tek Sorumluluk Prensibi (SRP):** Her sınıfın ve bileşenin sadece tek bir görevi olması hedeflendi. Örneğin, `Player` nesnesi, her biri farklı bir işlevden sorumlu olan `PlayerMovement`, `PlayerInteraction` ve `PlayerHolding` gibi küçük bileşenlere ayrıldı. Bu, kodun okunabilirliğini artırdı ve hata ayıklamayı kolaylaştırdı.

- **Açık/Kapalı Prensibi (OCP):** Sistem, genişlemeye açık ancak değişikliğe kapalı olacak şekilde tasarlandı. `IInteractable` arayüzü sayesinde `CuttingCounter` veya `StoveCounter` gibi yeni tezgah türleri eklendiğinde, oyuncunun etkileşim kodu olan `PlayerInteraction` üzerinde hiçbir değişiklik yapılması gerekmedi.

- **Arayüz Ayırma Prensibi (ISP):** Sınıfların, kullanmayacakları metotları uygulamaya zorlanmaması sağlandı. Örneğin, `Cut()` (Kesme) fonksiyonu, tüm tezgâhları etkilememesi için genel `IInteractable` arayüzünden ayrılarak, sadece ilgili tezgâhların uyguladığı `ICuttable` arayüzüne taşındı.

### 🧩 Uygulanan Tasarım Desenleri

- **Observer Deseni (Event Sistemi):** Projedeki en temel iletişim mekanizmasıdır. Sınıfların birbirine olan doğrudan bağımlılığı ortadan kaldırıldı.
    - `GameInput`, tuş girdilerini alıp `PlayerInteraction`'a olaylar (events) aracılığıyla bildirdi.
    - `RecipeManager`, yeni bir tarif oluştuğunda veya tamamlandığında `RecipeListUI`'a haber vererek arayüzün güncellenmesini sağladı.
    - `CuttingCounter`, kesme işlemi ilerlediğinde animasyon ve ilerleme çubuğu (progress bar) görsellerini olaylar aracılığıyla tetikledi.

- **State Machine Deseni (Durum Makinesi):** `StoveCounter` (Ocak) gibi karmaşık ve çok aşamalı nesnelerin mantığı bu desenle yönetildi. Ocağın `Idle`, `Frying`, `Fried`, ve `Burned` gibi durumları bir `enum` ile tanımlandı ve `Update()` metodu içinde o anki duruma göre farklı zamanlayıcılar ve dönüşüm mantıkları çalıştırıldı.

- **Singleton Deseni:** `RecipeManager` ve `GameInput` gibi tüm oyun boyunca tek olması gereken ve her yerden erişilmesi gereken yönetici sınıfları için kullanıldı.

### 💾 Veri Odaklı Tasarım: Scriptable Objects

- **Neden Kullanıldı?** Oyunun "verisini" (Malzemeler, Tarifler, Pişirme Kuralları) kodun mantığından tamamen ayırmak için kullanıldı. `KitchenObjectSO`, `RecipeSO`, `FryingRecipeSO` gibi Scriptable Object yapıları oluşturuldu.
- **Kazanım:** Bu yapı sayesinde, oyuna yeni bir malzeme (örn: "Salatalık") veya yeni bir yemek tarifi (örn: "Sandviç") eklemek için **tek bir satır kod yazmaya gerek kalmadı**. Sadece Unity Editor üzerinden yeni veri dosyaları (`.asset`) oluşturmak yeterli oldu. Bu, projenin esnekliğini ve genişletilebilirliğini en üst düzeye çıkardı.

---

## Projenin Kökeni ve Teşekkür

- **Orijinal Eğitim Serisi:** Bu proje, **Code Monkey**'in YouTube'daki popüler [**"Learn Unity Beginner/Intermediate 2025 (FREE COMPLETE Course - Unity Tutorial)"**](https://www.youtube.com/watch?v=AmGSEHGeY_I&list=PLzDRvYVwl53v5ur4F36i2_hIs-ffbCt1_) kursunun bir parçası olan "Kitchen Chaos" projesi temel alınarak geliştirilmiştir. Orijinal konsept ve temel mekanikler için tüm krediler Code Monkey'e aittir.

- **Geliştirme Sürecindeki Yardım:** Proje geliştirme sürecinde karşılaşılan mimari sorunların çözümünde, temiz kod prensiplerinin uygulanmasında, konseptlerin anlaşılmasında ve hata ayıklama süreçlerinde yapay zeka asistanı **Gemini**'dan yardım alınmıştır.

## 🚀 Projeyi Çalıştırma

1. Projeyi klonlayın.
2. Unity Hub üzerinden `Add` diyerek proje klasörünü seçin.
3. Ana sahne `Assets/Scenes/GameScene.unity` dosyasını açın ve Play tuşuna basın.
