# MzadPalestine - منصة المزاد الفلسطيني

MzadPalestine هي منصة مزادات إلكترونية متكاملة تتيح للمستخدمين إنشاء وإدارة المزادات، والمشاركة في المزايدة، وإدارة المحفظة المالية.

## المميزات الرئيسية

- نظام مزادات متكامل مع دعم للمزايدة المباشرة
- نظام محفظة إلكترونية للمدفوعات
- نظام رسائل ومحادثات بين المستخدمين
- نظام إشعارات فوري
- نظام تقييمات للمستخدمين
- دعم التصنيفات والمواقع
- نظام إدارة متكامل

## المتطلبات الأساسية

- .NET 7.0 SDK
- SQL Server (LocalDB or higher)
- Redis Server
- Node.js (for development)
- Visual Studio 2022 or VS Code

## التثبيت

1. استنسخ المشروع:
```bash
git clone https://github.com/yourusername/MzadPalestine.git
cd MzadPalestine
```

2. قم بتثبيت حزم NuGet:
```bash
dotnet restore
```

3. قم بتحديث قاعدة البيانات:
```bash
cd MzadPalestine.API
dotnet ef database update
```

4. قم بتعديل ملف `appsettings.json` وأضف:
- مفتاح JWT السري
- إعدادات البريد الإلكتروني
- مفاتيح Stripe للمدفوعات

5. قم بتشغيل التطبيق:
```bash
dotnet run
```

## الهيكل البرمجي

```
MzadPalestine/
├── MzadPalestine.API/          # واجهة برمجة التطبيقات
├── MzadPalestine.Core/         # نماذج البيانات والواجهات
├── MzadPalestine.Infrastructure/# التنفيذ والخدمات
└── MzadPalestine.Tests/        # اختبارات الوحدة
```

## الخدمات المستخدمة

- **المصادقة**: JWT مع Identity
- **قاعدة البيانات**: SQL Server مع Entity Framework Core
- **التخزين المؤقت**: Redis
- **المهام الخلفية**: Hangfire
- **الاتصال المباشر**: SignalR
- **المدفوعات**: Stripe

## المساهمة

نرحب بمساهماتكم! يرجى اتباع الخطوات التالية:

1. Fork المشروع
2. إنشاء فرع للميزة الجديدة
3. تقديم طلب Pull Request

## الترخيص

هذا المشروع مرخص تحت رخصة MIT. راجع ملف `LICENSE` للمزيد من المعلومات.

## الدعم

للمساعدة والاستفسارات، يرجى فتح issue في GitHub أو التواصل عبر البريد الإلكتروني.
