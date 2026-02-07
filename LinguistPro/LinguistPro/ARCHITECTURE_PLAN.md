# LinguistPro Application Architecture Restructuring Plan

## Current State
- Single database with shared data across all users
- No authentication/user profiles
- Data stored in simple models (VocabularyItem, VerbEntry, LanguageItem)

## Proposed Architecture

### 1. User Profile System
Create a User Profile management layer with:
- User authentication (username/password)
- Profile information (First Name, Last Name, Date of Birth, Country)
- User preferences

### 2. Profile-Based Language Databases
Each user profile will have:
- One or more language profiles (German, French, Spanish, etc.)
- Under each language profile:
  - Vocabulary database (VocabularyItems for that language)
  - Verb database (VerbEntries for that language)
  - Numbers database (LanguageItems - Numbers)
  - Days database (LanguageItems - Days)
  - Months database (LanguageItems - Months)

### 3. Database Structure Changes

#### New Models Needed:
1. **UserProfile**
   - UserId (PK)
   - Username (unique)
   - PasswordHash
   - FirstName
   - LastName
   - DateOfBirth
   - Country
   - CreatedDate
   - LastLoginDate

2. **LanguageProfile**
   - LanguageProfileId (PK)
   - UserId (FK)
   - LanguageCode (de, fr, es)
   - LanguageName
   - IsActive
   - CreatedDate
   - MasteryLevel (average of all items)

3. **Updated Models (add FK to LanguageProfile)**
   - VocabularyItem: Add LanguageProfileId FK
   - VerbEntry: Add LanguageProfileId FK
   - LanguageItem: Add LanguageProfileId FK
   - VerbEntry: Add LanguageProfileId FK

### 4. Implementation Steps

#### Phase 1: Create New Models
- Create UserProfile model
- Create LanguageProfile model
- Update existing models with FK relationships

#### Phase 2: Database Migration
- Create EF Core migration for new schema
- Migrate existing data (if any) to default profile

#### Phase 3: Authentication System
- Implement user registration
- Implement user login
- Implement session management
- Add authorization checks to all endpoints

#### Phase 4: Update UI/Pages
- Add login page
- Add profile management page
- Update Index page to work with selected profile/language
- Add language selection per profile

#### Phase 5: Update Business Logic
- Modify all CRUD operations to filter by current user
- Add multi-language support per user
- Implement mastery tracking per user

### 5. Security Considerations
- Hash passwords using BCrypt or ASP.NET Identity
- Use session/JWT tokens for authentication
- Implement authorization checks on all endpoints
- Add CSRF protection

### 6. Data Isolation
- All queries must include UserId filter
- Cannot access other users' data
- Language isolation per user

## Implementation Priority
1. HIGH: User authentication & profile management (blocking all other work)
2. HIGH: Database schema changes with migrations
3. MEDIUM: UI updates for login/profile
4. MEDIUM: Authorization on all endpoints
5. LOW: Advanced features (data backup, sharing, etc.)

## Notes
- Consider using ASP.NET Identity for user management (built-in solution)
- Current data will need migration strategy
- Session timeout policies needed
- Password reset functionality needed
