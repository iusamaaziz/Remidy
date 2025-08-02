# Remidy

**Comprehensive Medical Case Management Application for Homeopathic & Alternative Medicine Practitioners**

Remidy is a cross-platform medical case management application designed specifically for homeopathic and alternative medicine practitioners. It provides comprehensive patient case tracking, detailed medical assessments, treatment management, and metadata administration capabilities.

## 🏥 Features

### **Patient Case Management**
- **Complete Patient Records** - Comprehensive demographic and contact information
- **Detailed Medical History** - Extensive symptom tracking and assessment fields
- **Vital Signs Monitoring** - BP, temperature, pulse, respiration, BMI tracking
- **Physical Characteristics** - Constitution, nature, complexion, temperament assessment
- **Diagnostic Integration** - X-Ray, USG, CT Scan, MRI findings management

### **Medical Classification System**
- **18+ Lookup Types** for standardized medical data entry
- **Physical Assessment** - Hair, eye, face, nail, speaking, tongue characteristics
- **Treatment Tracking** - Treatment types, responses, and effectiveness monitoring
- **Causation Analysis** - Disease causes, sensations, and symptom patterns
- **Case Categorization** - Flexible categorization with color-coded organization

### **Task & Treatment Management**
- **Treatment Tasks** - Create and track treatment-related tasks per case
- **Progress Monitoring** - Mark tasks as completed to track treatment progress
- **Case Notes** - Detailed notes for peculiar, queer, and strange symptoms

### **Flexible Organization**
- **Tagging System** - Color-coded tags for flexible case organization
- **Category Management** - Hierarchical case categorization
- **Search & Filter** - Quick access to specific cases and data

### **Administrative Controls**
- **Metadata Management** - Full CRUD operations for all lookup types
- **Data Seeding** - Pre-populated medical classification data
- **Theme Support** - Light/dark mode interface
- **Cross-Platform** - iOS, Android, Windows, macOS support

## 🛠️ Technology Stack

### **Framework & Platform**
- **.NET 9 MAUI** - Cross-platform UI framework
- **C#** - Primary programming language
- **XAML** - User interface markup

### **Architecture & Patterns**
- **MVVM Pattern** - Model-View-ViewModel architecture
- **CommunityToolkit.Mvvm** - MVVM helpers and source generators
- **Dependency Injection** - Built-in .NET DI container

### **Data & Storage**
- **SQLite** - Local database storage
- **Microsoft.Data.Sqlite** - Database access layer
- **Repository Pattern** - Data access abstraction

### **UI & Experience**
- **Syncfusion.Maui.Toolkit** - Enhanced UI components
- **CommunityToolkit.Maui** - Additional MAUI extensions
- **Responsive Design** - Adaptive layouts for different screen sizes

## 📋 Prerequisites

- **.NET 9 SDK** or later
- **Visual Studio 2022** (17.8+) or **Visual Studio Code**
- **MAUI Workload** installed
- Platform-specific SDKs:
  - **Android SDK** (for Android development)
  - **Xcode** (for iOS/macOS development on Mac)
  - **Windows SDK** (for Windows development)

## 🚀 Installation & Setup

### **1. Clone Repository**
```bash
git clone https://github.com/yourusername/Remidy.git
cd Remidy
```

### **2. Restore Dependencies**
```bash
dotnet restore
```

### **3. Build Project**
```bash
dotnet build
```

### **4. Run Application**
```bash
# For Windows
dotnet run --framework net9.0-windows10.0.19041.0

# For Android (with device/emulator connected)
dotnet run --framework net9.0-android

# For iOS (Mac only, with device/simulator)
dotnet run --framework net9.0-ios
```

## 📁 Project Structure

```
Remidy/
├── Data/                    # Data Access Layer
│   ├── ProjectRepository.cs # Main patient case repository
│   ├── TaskRepository.cs    # Treatment task repository
│   ├── TagRepository.cs     # Tag management repository
│   ├── CategoryRepository.cs# Category management repository
│   ├── LookupRepository.cs  # Generic lookup type repository
│   ├── *TypeRepository.cs   # Specific lookup repositories
│   └── ILookup.cs          # Lookup interface definition
│
├── Models/                  # Data Models
│   ├── Project.cs          # Main patient case model
│   ├── ProjectTask.cs      # Treatment task model
│   ├── Category.cs         # Case category model
│   ├── Tag.cs              # Tag model
│   └── *Type.cs            # Medical lookup type models
│
├── Pages/                   # UI Pages
│   ├── MainPage.xaml       # Dashboard page
│   ├── ProjectListPage.xaml# Case records listing
│   ├── ProjectDetailPage.xaml# Individual case details
│   ├── TaskDetailPage.xaml # Task management
│   └── ManageMetaPage.xaml # Metadata administration
│
├── PageModels/             # MVVM ViewModels
│   ├── MainPageModel.cs    # Dashboard logic
│   ├── ProjectDetailPageModel.cs# Case detail logic
│   └── ManageMetaPageModel.cs# Metadata management logic
│
├── Services/               # Business Services
│   ├── SeedDataService.cs  # Data seeding service
│   └── ModalErrorHandler.cs# Error handling service
│
├── Resources/              # Application Resources
│   ├── AppIcon/           # Application icons
│   ├── Fonts/             # Custom fonts
│   ├── Images/            # Image assets
│   └── Splash/            # Splash screen assets
│
└── Platforms/             # Platform-specific code
    ├── Android/           # Android-specific implementations
    ├── iOS/               # iOS-specific implementations
    ├── MacCatalyst/       # macOS-specific implementations
    └── Windows/           # Windows-specific implementations
```

## 💊 Medical Data Model

### **Patient Case (Project)**
The core entity representing a complete patient case with:

- **Demographics** - Name, age, contact, family information
- **Vital Signs** - Blood pressure, temperature, pulse, respiration
- **Physical Assessment** - Height, weight, BMI, constitutional analysis
- **Medical History** - Comprehensive symptom tracking by body system
- **Treatment Records** - Treatment types, responses, effectiveness
- **Diagnostic Results** - Imaging and test findings

### **Lookup Types (Medical Classifications)**
Standardized medical data classifications:

| Category | Types |
|----------|-------|
| **Physical** | BMI Categories, Constitution, Nature, Look, Complexion |
| **Characteristics** | Temperament, Face, Hair, Eyes, Nails, Speaking, Tongue |
| **Medical** | Cause of Disease, Sensations, Treatments, Blood Groups |
| **Assessment** | Treatment Responses, Case Conditions, Personal Habits |

## 🎯 Usage Guide

### **Managing Cases**
1. **Create New Case** - Add patient demographics and initial assessment
2. **Record Symptoms** - Use detailed body system assessment forms
3. **Assign Classifications** - Select from standardized lookup types
4. **Track Treatment** - Add treatment tasks and monitor progress
5. **Document Results** - Record diagnostic findings and outcomes

### **Administrative Tasks**
1. **Manage Metadata** - Use "Manage Meta" page to edit lookup types
2. **Organize Cases** - Create categories and tags for organization
3. **Data Maintenance** - Reset and reseed application data when needed

### **Navigation**
- **Dashboard** - Overview of cases and quick statistics
- **Case Records** - Browse and search patient cases
- **Manage Meta** - Administrative control over medical classifications

## 🛡️ Data Security

- **Local Storage** - All data stored locally in SQLite database
- **No Cloud Dependency** - Complete offline functionality
- **Privacy First** - No data transmission or external dependencies
- **Backup Ready** - Database file can be backed up independently

## 🔧 Development

### **Adding New Lookup Types**
1. Create model class implementing `ILookup`
2. Add repository class extending `LookupRepository<T>`
3. Register in `MauiProgram.cs` dependency injection
4. Add UI section in `ManageMetaPage.xaml`
5. Add CRUD commands in `ManageMetaPageModel.cs`

### **Database Migrations**
The application uses SQLite with automatic table creation. Schema changes require:
1. Update table creation SQL in repositories
2. Update SELECT/INSERT/UPDATE operations
3. Consider data migration for existing installations

### **Building for Distribution**
```bash
# Android APK
dotnet publish -f net9.0-android -c Release

# iOS IPA (Mac only)
dotnet publish -f net9.0-ios -c Release

# Windows MSIX
dotnet publish -f net9.0-windows10.0.19041.0 -c Release
```

## 🤝 Contributing

1. **Fork** the repository
2. **Create** feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** changes (`git commit -m 'Add AmazingFeature'`)
4. **Push** to branch (`git push origin feature/AmazingFeature`)
5. **Open** Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support, issues, or feature requests:
- **GitHub Issues** - Report bugs and request features
- **Discussions** - General questions and community support

---

**Remidy** - Empowering alternative medicine practitioners with comprehensive digital case management tools.