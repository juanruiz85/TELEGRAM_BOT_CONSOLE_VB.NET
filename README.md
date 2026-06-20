# TELEGRAM_BOT_CONSOLE_VB.NET

Este proyecto es un bot de Telegram escrito en VB.NET como aplicación de consola. Se ha migrado a **.NET 8** para que funcione en **Windows** y **Linux** usando el SDK de .NET.

## Funciones

- Bot de Telegram basado en la librería `Telegram.Bot`
- Comandos básicos soportados:
  - `/now` : responde con la fecha y hora actual
  - `/myid` : devuelve el nombre y el ID del usuario
  - `/botones` : envía un teclado inline con botones de ejemplo
- Registro de actividad en archivos de log en `Logs/`
- Configuración del token desde variable de entorno `TELEGRAM_BOT_TOKEN` o desde el valor fijo en `Module1.vb`

## Cambios principales

- Migración a `net8.0` con proyecto SDK-style (`PackageReference`)
- Soporte multiplataforma: Linux y Windows
- Actualización de gestión de paquetes a `PackageReference`
- Uso de ruta de logs compatible con ambos sistemas operativos
- Mantiene `Telegram.Bot` y `Newtonsoft.Json` como dependencias

## Cómo usar

1. Instala el SDK de .NET 8 en tu sistema.
2. Clona o copia el proyecto.
3. Configura el token de bot:
   - Exporta la variable `TELEGRAM_BOT_TOKEN` en Linux/macOS:
     ```bash
     export TELEGRAM_BOT_TOKEN="TU_TOKEN_AQUI"
     ```
   - O define el valor en `Module1.vb` directamente.
4. Desde la raíz del repositorio ejecuta:
   ```bash
   dotnet restore TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj
   dotnet build TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj
   dotnet run --project TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj
   ```

## Ejecución

- En Linux/macOS:
  ```bash
  export TELEGRAM_BOT_TOKEN="TU_TOKEN_AQUI"
  dotnet run --project TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj
  ```
- En Windows PowerShell:
  ```powershell
  $env:TELEGRAM_BOT_TOKEN="TU_TOKEN_AQUI"
  dotnet run --project TELEGRAM_BOT_CONSOLE_VB.NET/TELEGRAM_BOT_CONSOLE_VB.NET.vbproj
  ```

## Dependencias

- `Telegram.Bot` 17.0.0
- `Newtonsoft.Json` 13.0.3

## Notas

- El proyecto ahora usa `.NET 8` y no depende de `packages.config` ni de `nuget.exe`.
- El bot procesa actualizaciones mediante polling con `GetUpdatesAsync` en lugar de los manejadores de evento legados.
- El archivo de log se crea en la carpeta `Logs` junto al ejecutable.
