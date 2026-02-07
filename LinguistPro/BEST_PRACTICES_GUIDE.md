# Lessons Learned - Best Practices Going Forward

## The Mistake

I added new properties to the `VerbEntry` model:
```csharp
public int Mastery { get; set; } = 0;
public DateTime LastReviewed { get; set; } = DateTime.UtcNow;
```

**Without** creating a database migration first.

Result: **Application crashed** with `SqliteException: no such column`

---

## Why This Happened

Entity Framework Core requires:
1. **Model classes** reflect the intended database schema
2. **Migrations** create the actual database changes
3. **Database** must be updated with `dotnet ef database update`

**The order matters**:
```
❌ WRONG SEQUENCE:
  1. Add property to model
  2. Write code using the property
  3. Run app → CRASH ❌

✅ CORRECT SEQUENCE:
  1. Add property to model
  2. Create migration
  3. Update database
  4. Write code using the property
  5. Run app → SUCCESS ✅
```

---

## How to Add Database Properties in Future

### Step 1: Modify the Model
```csharp
public class VerbEntry
{
    // ... existing properties ...
    
    public int Mastery { get; set; } = 0;  // NEW
}
```

### Step 2: Create a Migration
```bash
dotnet ef migrations add AddMasteryToVerbEntry
```

This generates a migration file in `LinguistPro/Migrations/` folder.

### Step 3: Update the Database
```bash
dotnet ef database update
```

### Step 4: NOW You Can Use It
```csharp
// This is safe now - column exists in database
var mastery = verbEntry.Mastery;
```

---

## Migration Best Practices

### Always Use Descriptive Migration Names
```bash
✅ GOOD:
   dotnet ef migrations add AddMasteryToVerbEntry
   dotnet ef migrations add AddUserAvatarUrl
   dotnet ef migrations add DropLegacyColumn

❌ BAD:
   dotnet ef migrations add Update
   dotnet ef migrations add Fix
```

### Check Generated Migration
```csharp
// Review the Up() method to ensure it's correct
public override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.AddColumn<int>(
        name: "Mastery",
        table: "Verbs",
        type: "INTEGER",
        nullable: false,
        defaultValue: 0);
}
```

### Test Migrations Locally First
```bash
# Remove the last migration if it's wrong
dotnet ef migrations remove

# Create a fresh one
dotnet ef migrations add ProperName
dotnet ef database update
```

---

## Database Evolution Workflow

### For Adding a New Property:
```
1. Modify Model → Add Property
2. Run: dotnet ef migrations add DescriptiveName
3. Review: Check Migrations/XXX_DescriptiveName.cs
4. Run: dotnet ef database update
5. Test: Verify in app
6. Commit: Git commit all changes
```

### For Removing a Property:
```
1. Modify Model → Remove Property
2. Run: dotnet ef migrations add RemoveXXX
3. Run: dotnet ef database update
4. Clean up: Remove usage from code
5. Test & Commit
```

### For Renaming a Property:
```
1. Modify Model → Change Property Name
2. Run: dotnet ef migrations add RenameXXXToYYY
3. Review migration (ensure data is preserved)
4. Run: dotnet ef database update
5. Update queries/code to use new name
```

---

## Current Database Schema

### VerbEntry Table (Current)
```sql
CREATE TABLE VerbEntry (
    Id INTEGER PRIMARY KEY,
    LanguageProfileId INTEGER,
    Language TEXT NOT NULL,
    Infinitive TEXT NOT NULL,
    Meaning TEXT,
    Tense TEXT,
    S1 TEXT,
    S2Inf TEXT,
    S2Form TEXT,
    S3 TEXT,
    P1 TEXT,
    P2Inf TEXT,
    P2Form TEXT,
    P3 TEXT
    -- NO Mastery column (don't try to query it!)
    -- NO LastReviewed column (don't try to query it!)
);
```

### VocabularyItem Table (For Reference)
```sql
CREATE TABLE VocabularyItem (
    Id INTEGER PRIMARY KEY,
    LanguageProfileId INTEGER,
    Language TEXT,
    Term TEXT,
    Meaning TEXT,
    Definition TEXT,
    UsageExample TEXT,
    UsageExampleMeaning TEXT,
    Mastery INTEGER,        -- This one HAS it
    LastReviewed DATETIME   -- This one HAS it
);
```

---

## How to Avoid This in Future

### Checklist Before Running Application

- [ ] Model property added?
- [ ] Does it require database schema change?
- [ ] Is there a migration file created?
- [ ] Did you run `dotnet ef database update`?
- [ ] Does the column exist in database?
- [ ] Does code compile without errors?
- [ ] Does application run without crashing?

### Code Review Questions

```
When reviewing code that adds model properties:

1. "Is this property mapped to a database column?"
2. "If yes, is there a corresponding migration?"
3. "Was the database updated with this migration?"
4. "Does the code compile and run?"
5. "Are there any database query errors?"
```

---

## Reference: EF Core Workflow

### Initialize Migrations (One-time)
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Add a Property
```bash
# Edit Model.cs
# Add new property

# Create migration
dotnet ef migrations add AddPropertyName

# Apply to database
dotnet ef database update
```

### View Migration History
```bash
dotnet ef migrations list
```

### View Applied Migrations on Database
```bash
SELECT MigrationId FROM __EFMigrationsHistory;
```

---

## Key Takeaway

**Always maintain alignment**:
```
Model (C# Classes)
    ↓ (must match)
Migrations (SQL Scripts)
    ↓ (must match)
Database Schema (Actual Tables)
    ↓ (must match)
Database Queries (SELECT/INSERT/UPDATE)
```

If any part is out of sync → **Application crashes**

---

## For This Project Going Forward

### When Adding Verb Mastery in the Future:

1. ✅ Add properties to VerbEntry model
2. ✅ Create migration: `dotnet ef migrations add AddMasteryToVerbEntry`
3. ✅ Update database: `dotnet ef database update`
4. ✅ Update AnalyticsService to calculate verb mastery
5. ✅ Update Analytics.cshtml.cs to use verb mastery data
6. ✅ Test thoroughly
7. ✅ Commit with clear message

---

## Emergency Recovery (What We Did)

If you make this mistake again and need to quickly fix:

1. **Remove the problematic code**:
   - Remove property from model
   - Remove any code using the property
   - **Don't** try to create a migration

2. **Build and verify**:
   - `dotnet build` - must succeed
   - No compilation errors

3. **Run application**:
   - Should work again

4. **Plan proper migration later**:
   - When you're ready, follow the proper process

---

**Remember**: Database migrations are your safety net. Use them!
