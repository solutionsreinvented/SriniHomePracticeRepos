# Database Migration Guide for User Profile Implementation

## Current State
- AppDbContext with 3 DbSets: Vocabulary, Verbs, LanguageItems
- No user authentication or profiles
- All data is shared globally

## Planned Migration Path

### Step 1: Add New Models to DbContext
Add these DbSets to AppDbContext:
```csharp
public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
public DbSet<LanguageProfile> LanguageProfiles => Set<LanguageProfile>();
```

### Step 2: Update Existing Models (Add Foreign Keys)

**VocabularyItem.cs** - Add these properties:
```csharp
[ForeignKey(nameof(LanguageProfile))]
public int LanguageProfileId { get; set; }

public LanguageProfile? LanguageProfile { get; set; }
```

**VerbEntry.cs** - Add these properties:
```csharp
[ForeignKey(nameof(LanguageProfile))]
public int LanguageProfileId { get; set; }

public LanguageProfile? LanguageProfile { get; set; }
```

**LanguageItem.cs** - Add these properties:
```csharp
[ForeignKey(nameof(LanguageProfile))]
public int LanguageProfileId { get; set; }

public LanguageProfile? LanguageProfile { get; set; }
```

### Step 3: Create EF Core Migration

```bash
dotnet ef migrations add AddUserProfiles
```

This will:
1. Create new UserProfile and LanguageProfile tables
2. Add LanguageProfileId foreign keys to existing tables
3. Generate migration file in Migrations folder

### Step 4: Apply Migration

```bash
dotnet ef database update
```

### Step 5: Data Migration Strategy (if you have existing data)

Option A - Assign all existing data to a "Default" profile:
```csharp
// Create default user profile
var defaultUser = new UserProfile 
{ 
    Username = "admin",
    PasswordHash = "hash_here",
    FirstName = "Default",
    LastName = "User"
};
dbContext.UserProfiles.Add(defaultUser);
await dbContext.SaveChangesAsync();

// Create language profiles for each language
var deProfile = new LanguageProfile 
{ 
    UserId = defaultUser.UserId,
    LanguageCode = "de",
    LanguageName = "German"
};
dbContext.LanguageProfiles.Add(deProfile);
await dbContext.SaveChangesAsync();

// Update existing vocabulary items
var vocabItems = await dbContext.Vocabulary.ToListAsync();
foreach (var item in vocabItems)
{
    if (item.Language == "de")
        item.LanguageProfileId = deProfile.LanguageProfileId;
    // ... similar for other languages
}
await dbContext.SaveChangesAsync();
```

Option B - Keep data separate (recommended for fresh start):
1. Migrate only the schema
2. Start with fresh user registration
3. Old data remains accessible if needed (create archive table)

### Step 6: Update DbContext Configuration

Add navigation property configuration in OnModelCreating:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Configure relationships
    modelBuilder.Entity<UserProfile>()
        .HasMany(u => u.LanguageProfiles)
        .WithOne(l => l.UserProfile)
        .HasForeignKey(l => l.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder.Entity<LanguageProfile>()
        .HasMany(l => l.VocabularyItems)
        .WithOne(v => v.LanguageProfile)
        .HasForeignKey(v => v.LanguageProfileId)
        .OnDelete(DeleteBehavior.Cascade);
    
    // ... similar for VerbEntry and LanguageItem
}
```

## Migration Checklist

- [ ] Create UserProfile.cs model
- [ ] Create LanguageProfile.cs model
- [ ] Update AppDbContext with new DbSets
- [ ] Add FK properties to VocabularyItem
- [ ] Add FK properties to VerbEntry
- [ ] Add FK properties to LanguageItem
- [ ] Create EF migration: `dotnet ef migrations add AddUserProfiles`
- [ ] Apply migration: `dotnet ef database update`
- [ ] Test that existing functionality still works
- [ ] Implement data migration strategy if needed
- [ ] Update all CRUD operations to filter by LanguageProfileId
- [ ] Add authentication system (ASP.NET Identity or custom)

## Expected Database Schema After Migration

```
UserProfiles
├── UserId (PK)
├── Username (unique)
├── PasswordHash
├── FirstName
├── LastName
├── DateOfBirth
├── Country
├── CreatedDate
└── LastLoginDate

LanguageProfiles
├── LanguageProfileId (PK)
├── UserId (FK → UserProfiles)
├── LanguageCode
├── LanguageName
├── MasteryLevel
├── CreatedDate
└── IsActive

VocabularyItems (updated)
├── Id
├── LanguageProfileId (FK → LanguageProfiles) ← NEW
├── Term
├── Meaning
├── Language
├── Mastery
└── LastReviewed

VerbEntries (updated)
├── Id
├── LanguageProfileId (FK → LanguageProfiles) ← NEW
├── Language
├── Infinitive
└── ... other fields

LanguageItems (updated)
├── Id
├── LanguageProfileId (FK → LanguageProfiles) ← NEW
├── ItemType
├── Term
├── Meaning
└── ... other fields
```

## Rollback Plan

If you need to rollback:
```bash
# Remove the migration (if not applied)
dotnet ef migrations remove

# Or rollback the database
dotnet ef database update <previous_migration_name>
```

## Testing After Migration

1. Verify tables created correctly
2. Create test user and language profile
3. Add vocabulary items to language profile
4. Verify relationships work correctly
5. Test CRUD operations with new structure
6. Verify cascade deletes work

## Notes

- Keep the old schema in a backup database initially
- Test in development environment first
- Plan downtime for production migration
- Have a rollback plan ready
- Consider data archival for old/unused data
