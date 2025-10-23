# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

RazorReaper is a Blazor Hybrid (.NET MAUI) desktop application for managing ARK: Survival Evolved configuration files and server utilities. It's the successor to ReaperV2 (a WinForms tool), built with modern web technologies embedded in a native Windows application.

**Key Technologies:**
- .NET 9.0 (net9.0-windows10.0.19041.0)
- Blazor Hybrid with MAUI WebView
- Microsoft.Maui.Controls 9.0.90
- Target platform: Windows 10+

## Development Commands

### Build
```bash
dotnet build RazorReaper.sln
```

### Run (Debug)
```bash
dotnet run --project RazorReaper/RazorReaper.csproj
```

### Clean
```bash
dotnet clean RazorReaper.sln
```

### Restore Dependencies
```bash
dotnet restore RazorReaper.sln
```

## Architecture

### Application Entry Points
- **MauiProgram.cs**: Application bootstrapping, service registration, and font configuration
- **App.xaml.cs**: MAUI application lifecycle, creates the main window
- **MainPage.xaml**: Hosts the BlazorWebView component that renders Blazor content

### Blazor Component Structure
```
Components/
├── Routes.razor          # Router configuration
├── _Imports.razor        # Global using directives
├── Layout/
│   └── MainLayout.razor  # Main layout wrapper
├── Pages/                # Feature pages (Blazor components)
│   ├── Home.razor        # Dashboard with system status
│   ├── IniChanger.razor  # BaseDeviceProfiles.ini editor with presets
│   ├── Server.razor      # Server connection via Battle Matrix
│   ├── Game.razor        # Game-related tools
│   ├── SuitFov.razor     # FOV adjustments
│   ├── Autoclicker.razor # Autoclicker functionality
│   ├── Pixel.razor       # Pixel-related tools
│   ├── Building.razor    # Building tools
│   ├── Paintings.razor   # Painting tools
│   └── Credits.razor     # About/credits page
└── Shared/
    └── SharedNavbar.razor # Navigation component
```

### Static Assets
```
wwwroot/
├── index.html            # WebView host page with JS interop
├── css/                  # Per-page stylesheets + theme.css
├── js/                   # JavaScript files (e.g., network-animation.js)
├── images/               # Image assets
└── videos/               # Video assets
```

### Utilities
- **ArkUtilities.cs**: Windows Registry-based ARK installation detection
  - `FindArkPath()`: Scans Steam registry and libraryfolders.vdf to locate ARK installation
  - `GetBaseDeviceProfilesPath()`: Returns path to BaseDeviceProfiles.ini config file

### JavaScript Interop
The application uses JS interop for:
- `startLocationPicker()`: Crosshair-style position picker with click capture (used in autoclicker features)
- Network animation canvas rendering

## Important Implementation Details

### ARK Path Detection
The app automatically finds ARK installations by:
1. Reading Steam install path from Windows Registry (`HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam` or `HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam`)
2. Parsing `steamapps/libraryfolders.vdf` for additional library folders
3. Validating ARK existence by checking for `ShooterGame/Binaries/Win64/ShooterGame.exe`
4. Target config file: `Engine/Config/BaseDeviceProfiles.ini`

### Blazor + MAUI Integration
- The MAUI ContentPage hosts a BlazorWebView component
- Blazor components are rendered inside the WebView, not native MAUI controls
- Router is configured in Routes.razor with MainLayout as the default layout
- Pages use `@page` directive for routing (e.g., `@page "/ini-changer"`)

### Styling Approach
- Dark theme with black/purple color scheme
- Global theme in `wwwroot/css/theme.css`
- Per-page CSS files matching component names (e.g., `home-styles.css` for Home.razor)
- All stylesheets linked in `index.html`

### BaseDeviceProfiles.ini Editor
The IniChanger page provides:
- File operations: Load, save, check path, open in default editor
- 10 optimized presets for different use cases (Bloodstalker, Gen2 Farming, Ultra Minimal FPS, etc.)
- Direct file manipulation via System.IO

## Platform-Specific Code

Platform-specific implementations exist in:
```
Platforms/
├── Windows/      # Windows-specific (App.xaml, manifest, etc.)
├── Android/      # Android support (future)
├── iOS/          # iOS support (future)
├── MacCatalyst/  # macOS support (future)
└── Tizen/        # Tizen support (future)
```

Currently targets Windows only, but MAUI provides cross-platform scaffolding.

## Configuration Files

- **RazorReaper.csproj**: Project file with MAUI/Blazor package references
- **Properties/launchSettings.json**: Launch profiles
- **App.xaml**: MAUI application resources
- **MainPage.xaml**: BlazorWebView host configuration

## Project Structure Notes

- Solution file: `RazorReaper.sln` (single project)
- Output type: Executable (Windows desktop app)
- Root namespace: `RazorReaper`
- Uses top-level statements: No (MauiProgram has explicit class)
- Nullable reference types: Enabled
