# DotNetArcs - is a multi-compression lib

## EN
This lib is has forked Ionic.Zlib and SharpZipLib (#ziplib, formerly NZipLib) compression library's for Zip, GZip, BZip2, Zlib, and Tar by VSS-DEV for compile to .NET Standard 2.0|2.1

## RU
Эта библиотека содержит форк библиотек Ionic.Zlib и SharpZipLib для осуществления компрессии в алгоритмах Zip, GZip, BZip2, Zlib, и Tar. Скомпилирована для использования в .NET Standard 2.0|2.1

# Использование библиотеки
Библиотека позволяет пользоваться и формировать на основе функций из библиотек Ionic.Zlib и SharpZipLib собственные решения по компрессии или декомпрессии данных имея "классические" ссылки на решения.

Например:
```cs
using Ionic.Zlib;
```

Однако есть и готовый способ. Достаточно просто добавить решение:
``` cs
using DotNetArcs;
```
и воспользоваться следующим вызовом функции:
``` cs

new Archives(ArchivFormat,ArchivFunction,DataInByte, out DataArcInByte);

```
где:
``` cs
/// <param name="ArchivFormat", type = "enum">Выбор формата архива</param>
/// <param name="ArchivFunction", type = "enum">Что будем делать с данными</param>
/// <param name="DataInByte", type = "byte[]">Данные с которыми будет работать функция</param>
/// <param name="DataArcInByte", type = "byte[]">Результат который возвращает функция</param>
```







