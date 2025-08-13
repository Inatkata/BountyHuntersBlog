# BountyHuntersBlog

BountyHuntersBlog е ASP.NET Core MVC уеб приложение, разработено като финален проект по курса **ASP.NET Advanced – June 2025 @ SoftUni**.  
Приложението представлява блог платформа за "ловци на глави", където потребители могат да публикуват мисии, да добавят коментари и харесвания, както и да филтрират съдържанието по категории и тагове.  

## 🛠 Технологии и инструменти
- **ASP.NET Core 8.0 MVC**
- **Entity Framework Core** с **MS SQL Server**
- **ASP.NET Identity** (роли: `User` и `Admin`)
- **AutoMapper** за мапване на модели
- **Bootstrap 5** за responsive UI
- **Repository & Service Layer архитектура**
- **Dependency Injection**
- **Razor View Engine** с layout-и и partial-и
- **Pagination & Search/Filter**
- **Custom Error Pages** (404, 500)
- **Validation** (Client & Server Side)

## 📌 Основни функционалности
- Регистрация и логин с ролеви достъп
- CRUD операции за мисии, коментари, категории и тагове
- Харесвания на мисии и коментари
- Филтриране и търсене по ключова дума, категория или таг
- Админ панел (Areas/Admin) за модерация на съдържание
- Защитен достъп с `Authorize` атрибути
- Seed-нати категории и тагове

## 📂 Архитектура
```

BountyHuntersBlog/
│
├── BountyHuntersBlog.Data         # Entity модели, DbContext, Fluent Configurations
├── BountyHuntersBlog.Repositories # Repository pattern
├── BountyHuntersBlog.Services     # Service layer + DTOs
├── BountyHuntersBlog.ViewModels   # View модели
├── BountyHuntersBlog.Web          # Controllers, Views, MappingProfile
└── ...

````

## 🚀 Стартиране на проекта
1. Клонирай репото:
   ```bash
   git clone https://github.com/username/BountyHuntersBlog.git
````

2. Конфигурирай `appsettings.json` за твоята база:

   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=BountyHuntersBlog;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
3. Мигрирай базата:

   ```bash
   dotnet ef database update
   ```
4. Стартирай приложението:

   ```bash
   dotnet run
   ```

## 🧪 Тестове

* Unit тестове покриват над 65% от бизнес логиката (Service Layer)
* Използван е **Moq** за mocking на зависимости

## 🔒 Сигурност

* AntiForgery Tokens
* XSS & SQL Injection защита
* Валидация на данни на ниво модел и вход

## 📜 Лиценз

MIT License – свободно за използване и модификация

```
