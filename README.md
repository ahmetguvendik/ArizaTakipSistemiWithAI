# Arıza Takip Sistemi with AI

Bu proje, yapay zeka destekli bir **Arıza Takip Sistemi**'dir. Amaç; işletmelerin arıza kayıt süreçlerini kolaylaştırmak, kullanıcı deneyimini artırmak ve yapay zeka ile arıza sınıflandırma süreçlerini otomatikleştirmektir.

## 🔧 Proje Özellikleri

- Kullanıcı tabanlı giriş/çıkış sistemi
- Arıza kaydı oluşturma, listeleme, güncelleme, silme işlemleri (CRUD)
- Roller (Supervisor / Teknisyen) bazlı yetkilendirme
- Yapay zeka ile arıza önemi tahmini
- ASP.NET Core ile RESTful API mimarisi
- CSHTML tabanlı frontend
- Entity Framework Core ile veri erişimi

## 🧠 AI Özelliği

Arıza açıklamaları üzerinden yapay zeka modeli kullanılarak arızanın hangi kategoriye ait olduğu otomatik olarak tahmin edilmektedir. Bu sayede kullanıcıların doğru kategoriyi seçmesi kolaylaştırılır.

> Not: Yapay zeka modeli `Semantic Kernal` ile dış servis ile entegre edilmiştir. Daha fazla detay için `AIService` klasörüne göz atabilirsiniz.

## 🚀 Teknolojiler

- ASP.NET Core 9
- Entity Framework Core
- Razor Pages / CSHTML
- Postgres Server
- Semantic Kernel ve OpenRouter ile yapay zeka entegrasyonu
- Bootstrap (temel stil için)
- LINQ, AutoMapper, Dependency Injection
- Fluent Validation
- SignalR

## 🖼️ Ekran Görüntüleri

![9](https://github.com/user-attachments/assets/7df6d915-bbb2-43e2-8ec4-faffe5791cab)
![8](https://github.com/user-attachments/assets/230bf573-8743-4692-ab09-63c1f676feff)
![7](https://github.com/user-attachments/assets/142d5122-12f1-486d-9cac-78d8042b6675)
![6](https://github.com/user-attachments/assets/33afef14-871e-48bb-9483-1072507642ca)
![5](https://github.com/user-attachments/assets/57fbef5d-69d6-4403-842d-00def387b306)
![4](https://github.com/user-attachments/assets/24975bc1-e3a9-4500-a487-3f5cad09e19c)
![3](https://github.com/user-attachments/assets/924cf820-fb28-4807-ae1b-1dd9c03735f6)
![2](https://github.com/user-attachments/assets/d0b3dafc-878f-4767-bf43-eb72a8fe50ed)
![1](https://github.com/user-attachments/assets/daf1b310-1d03-4ef9-b8bf-e95eb36b0e67)


## 🛠️ Kurulum

1. Bu repoyu klonlayın:

```bash
git clone https://github.com/ahmetguvendik/ArizaTakipSistemiWithAI.git
