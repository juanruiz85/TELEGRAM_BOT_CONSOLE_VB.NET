# Changelog

## [Unreleased]

### Migración a .NET 8

- Migrado el proyecto VB.NET a `.NET 8` usando el formato SDK-style (`Project Sdk="Microsoft.NET.Sdk"`).
- Actualizado el archivo de proyecto `TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj` para usar `<TargetFramework>net8.0</TargetFramework>`.
- Reemplazada la gestión de dependencias heredada (`packages.config`) por `PackageReference`.
- Eliminado el archivo `nuget.exe` y la dependencia de paquetes NuGet legacy en el repositorio.
- Incluidas las dependencias:
  - `Telegram.Bot` 17.0.0
  - `Newtonsoft.Json` 13.0.3

### Compatibilidad multiplataforma

- El proyecto ahora es compatible con Windows y Linux usando el SDK de .NET.
- Se eliminó la dependencia de `xbuild` / Mono `vbnc` / `vbc` para el build en Linux.
- La ruta de logs se cambió a una carpeta `Logs` junto al ejecutable usando `AppContext.BaseDirectory`.
- Se verificó compilación exitosa con `dotnet build` en `net8.0`.

### Cambios en el código

- `Module1.vb` ahora carga el token desde la variable de entorno `TELEGRAM_BOT_TOKEN`.
- Si no existe la variable de entorno, se usa el valor de marcador de posición `TU TOKEN AQUI`.
- El bot procesa actualizaciones mediante polling con `GetUpdatesAsync` y no depende de eventos de Telegram.Bot legados.
- Se mejoró el manejo de mensajes no textuales y se normalizó el mensaje de comando no reconocido.
- Se añadió un mensaje de inicio para indicar al usuario que presione ENTER para detener el bot.
- Se mejoró el registro de errores y la salida de información en consola.

### Documentación

- Actualizado `README.md` con las nuevas instrucciones de compilación y uso para `.NET 8`.
- Agregada explicación de las funciones actuales del bot, dependencias y recomendaciones para ejecutar en Linux y Windows.
