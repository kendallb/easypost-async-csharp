@echo off
rmdir /S /Q NuGet
.nuget\NuGet pack EasyPost -Symbols -SymbolPackageFormat snupkg -Properties Configuration=Release -OutputDirectory NuGet