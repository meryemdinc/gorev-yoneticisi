# gorev-yoneticisi
-ASP.NET Core Web API ile JWT ve Swagger destekli Görev Yöneticisi Uygulaması-

Görev Yöneticisi Projesi, ASP.NET Core ile geliştirilen ve kullanıcıların günlük, haftalık ve aylık görevlerini planlayarak takip etmelerini sağlayan bir Web API uygulamasıdır. JWT tabanlı kimlik doğrulama sistemiyle güvenli bir kullanıcı yönetimi sağladım. Projeyi geliştirirken çok katmanlı mimari prensiplerini benimsedim; controller, service, repository ve DTO katmanlarını net şekilde ayırarak okunabilirliği ve sürdürülebilirliği artırdım.

Kullanıcılar sisteme kayıt olup giriş yaptıktan sonra bir JWT token alabiliyor ve bu token ile yalnızca yetkilendirilmiş işlemleri gerçekleştirebiliyor. Görev ekleme, silme, güncelleme ve listeleme gibi işlemler sadece kimliği doğrulanmış kullanıcılar tarafından yapılabiliyor. Veri yönetimi için Entity Framework Core kullanarak MSSQL veritabanı ile çalıştım. Swagger arayüzü üzerinden tüm API uç noktalarını test edilebilecek şekilde entegre ettim ve JWT ile doğrulama akışını Swagger’a tanımladım.

Bu proje, hem kendi backend geliştirme becerilerimi geliştirdiğim, hem de gerçek dünya uygulamalarına kolayca entegre edilebilecek bir yapı sunduğum bir çalışma oldu.
NOT:Uygulamayı test etmek için Swagger arayüzüne giderek JWT token ile giriş yaptıktan sonra API uç noktalarını doğrudan test edebilirsiniz.
