# ArmaRadio - Music Streaming & Discovery Platform

A comprehensive music streaming system showcasing full-stack development expertise. This portfolio project demonstrates architecture design, distributed systems, mobile development, and data processing at scale.

## 🎯 System Overview

ArmaRadio is a feature-rich music platform that combines streaming capabilities with intelligent music discovery, offline listening, and large-scale music metadata processing. The system integrates multiple services that work together to provide a complete music experience.

## 📦 Project Components

### armaradio - Core Web Application

**ASP.NET Core 8 MVC Web Application**
The main user interface for music streaming and playlist management. Features include:
- Real-time music streaming with adaptive bitrate support
- Dynamic playlist creation and management
- User authentication and profile management
- Music recommendation engine integration
- Responsive web design with modern UI/UX patterns

**Key Technologies:** ASP.NET Core MVC, Entity Framework Core, JWT Authentication

### arma_historycompiler - Listening History Service

**Background Service / Windows Service**
Continuously syncs user listening data from external music tracking services:
- Integrates with ListenBrainz API (open-source Last.fm alternative)
- Processes and normalizes listening history from multiple sources
- Stores timestamped listening data in SQL database for analytics
- Supports batch processing for efficient data synchronization

**Key Technologies:** .NET Worker Service, REST API Integration, Database Bulk Operations

### arma_miner - Music Metadata Service

**Background Service / Windows Service**
Maintains comprehensive music metadata database by syncing with music brainz:
- Fetches artist, album, and track information from MusicBrainz data dumps
- Handles data normalization and deduplication
- Maintains relationship mapping between artists, albums, and tracks
- Supports incremental updates to minimize API load

**Key Technologies:** .NET Worker Service, MusicBrainz data, Entity Relationship Management, Data Validation

### armaradio_ops - Database Seeding Service

**One-time Data Migration Tool**
Initial population system for music database:
- Bulk import of initial artist/album catalog
- Data transformation and cleanup operations
- Reference data initialization
- Migration scripting and version control

**Key Technologies:** .NET Console Application, SQL Bulk Copy, Data Transformation Logic

### armaoffline - Mobile Application

**Cross-platform Mobile App (Android/iOS)**
Enables offline music playback and synchronization:
- Download playlists for offline listening
- Background audio playback
- Local storage management
- Sync engine for playlist updates
- Native mobile UX patterns

**Key Technologies:** Xamarin/.NET MAUI, SQLite, Background Services, Media Player APIs

### Supporting Projects

**arma_genregenerator** (Legacy)
Originally designed for music genre classification and tagging based on audio analysis and metadata patterns.

**armaradioSongsAlike** (Legacy)
Early prototype for song similarity analysis and recommendation algorithms, later integrated into main platform.

## 🗄️ Database Architecture

The SQL folder contains comprehensive database schemas including:

### Core Tables
- `Users` & `Profiles` - User management and preferences
- `Artists`, `Albums`, `Tracks` - Music catalog
- `Playlists` & `PlaylistTracks` - User-generated content
- `ListeningHistory` - User activity tracking

### Specialized Components
- **Stored Procedures:** Optimized data operations and complex queries
- **User-Defined Functions:** Reusable logic for music analysis
- **Indexed Views:** Performance optimization for common queries
- **Full-Text Search:** Advanced music search capabilities

## 🔧 Configuration

**Important Security Note:** All sensitive configuration files have been omitted. You must create your own `appsettings.json` files with appropriate connection strings and API credentials.

### Main Application Configuration (`armaradio/appsettings.json`):
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "radioconn": "[Primary Music Database]",
    "adminconn": "[Administrative Database]",
    "recommendations": "[Analytics Database]"
  },
  "ApplicationConfiguration": {
    "apiClientId": "[External API Credentials]",
    "apiClientSecret": "[External API Credentials]"
  }
}
```

## 🚀 Architecture Highlights

### Design Patterns Implemented

- **Microservices Architecture:** Decoupled services with specific responsibilities
- **Repository Pattern:** Data access abstraction
- **Strategy Pattern:** Multiple algorithm implementations (recommendations, sorting)
- **Observer Pattern:** Real-time update propagation
- **Factory Pattern:** Service instantiation management

### Performance Features
- Caching strategies at multiple layers
- Database connection pooling
- Asynchronous processing throughout
- Efficient data pagination
- Optimized media streaming

### Scalability Considerations
- Stateless web application design
- Horizontally scalable services
- Database sharding strategy for user data
- Queue-based processing for background tasks

## 📊 Data Flow

1. **Metadata Pipeline:** MusicBrainz → arma_miner → SQL Database
2. **User Activity:** Listening clients → ListenBrainz → arma_historycompiler → SQL Database
3. **Content Delivery:** SQL Database → armaradio Web → User browsers
4. **Offline Sync:** armaradio Web → armaoffline Mobile → Local storage

## 🛡️ Security Implementation

- OAuth 2.0 for external API authentication
- JWT-based session management
- Encrypted connection strings
- SQL injection prevention via parameterized queries
- Role-based access control (RBAC)
- Secure file validation

## 🔄 Development Practices Showcased

- **CI/CD Ready:** Structured for automated deployment
- **Comprehensive Logging:** Structured logging with correlation IDs
- **Error Handling:** Global exception handling with graceful degradation
- **Testing:** Unit test structure (test projects omitted)
- **Documentation:** In-code documentation
- **Code Quality:** Consistent naming conventions and SOLID principles

## 💡 Portfolio Value

This project demonstrates:
- **Full-Stack Proficiency:** Frontend to backend to mobile
- **System Design:** Multi-service architecture with clear boundaries
- **API Integration:** Working with third-party REST APIs
- **Data Modeling:** Complex relational database design
- **Performance Optimization:** At application and database levels

---

_Note: This is a portfolio project showcasing development skills. Some components have been simplified or redacted for security and intellectual property protection. The system is not intended for production deployment without significant security and scalability enhancements._
