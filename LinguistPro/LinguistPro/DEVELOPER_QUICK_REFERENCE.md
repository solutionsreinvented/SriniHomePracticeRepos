# LinguistPro Developer Quick Reference - User Profile Implementation

## 🚀 QUICK START FOR TESTING

### 1. Run the Application
```bash
cd LinguistPro
dotnet run
```

### 2. Navigate to Application
```
http://localhost:5000  → Auto-redirects to Login
```

### 3. Register New User
- Click "Sign Up" on Login page
- Enter:
  - First Name: John
  - Last Name: Doe
  - Email: john@example.com
  - Password: Password1 (min 6, uppercase, digit)
  - Date of Birth: (optional)
  - Country: USA (optional)
- Click "Create Account"

### 4. Auto-Created Resources
After registration, the system automatically creates:
```
✅ ApplicationUser (john@example.com)
✅ UserProfile (John Doe, DOB, Country)
✅ LanguageProfile: German (de) - Active by default
✅ LanguageProfile: French (fr)
✅ LanguageProfile: Spanish (es)
```

### 5. Test Data Isolation
- Add vocabulary to German: "Haus" → "House"
- Switch language to French (via SelectedLanguage parameter)
- French vocabulary list is empty ✓
- Switch back to German
- German vocabulary shows "Haus" ✓

---

## 🔑 KEY CODE PATTERNS

### Get Current User ID
```csharp
var userId = await GetCurrentUserId();
```

### Get Current User's Language Profile
```csharp
var langProfile = await _db.LanguageProfiles
    .FirstOrDefaultAsync(l => l.UserId == userId && 
                             l.LanguageCode == SelectedLanguage);
```

### Query Data for Current User Only
```csharp
var items = await _db.Vocabulary
    .Where(v => v.LanguageProfileId == langProfile.LanguageProfileId)
    .ToListAsync();
```

### Add Item with User Isolation
```csharp
var item = new VocabularyItem
{
    Term = "Haus",
    Meaning = "House",
    Language = "de",
    LanguageProfileId = langProfile.LanguageProfileId  // Important!
};
_db.Vocabulary.Add(item);
await _db.SaveChangesAsync();
```

### Update with Ownership Check
```csharp
var item = await _db.Vocabulary
    .FirstOrDefaultAsync(v => v.Id == itemId && 
                             v.LanguageProfileId == langProfile.LanguageProfileId);

if (item != null)
{
    item.Meaning = "newMeaning";
    await _db.SaveChangesAsync();
}
```

---

## 📊 DATA MODEL DIAGRAM

```
┌──────────────────────────────────────────────────────────┐
│                    ASP.NET Identity                      │
│                  ┌─────────────────┐                    │
│                  │ AspNetUsers     │                    │
│                  │ (Email, Hash)   │                    │
│                  └────────┬────────┘                    │
└───────────────────────────┼─────────────────────────────┘
                            │ 1-to-1
┌───────────────────────────▼─────────────────────────────┐
│                      UserProfile                         │
│ ┌─────────────────────────────────────────────────────┐ │
│ │ UserId (PK, FK)                                     │ │
│ │ FirstName, LastName                                 │ │
│ │ DateOfBirth, Country                                │ │
│ │ CreatedDate, LastLoginDate                          │ │
│ │ IsActive                                            │ │
│ └──────────────────────────┬──────────────────────────┘ │
└────────────────────────────┼──────────────────────────────┘
                             │ 1-to-Many
┌────────────────────────────▼──────────────────────────────┐
│                   LanguageProfile                         │
│ ┌──────────────────────────────────────────────────────┐ │
│ │ LanguageProfileId (PK)                               │ │
│ │ UserId (FK)                                          │ │
│ │ LanguageCode (de/fr/es)                              │ │
│ │ LanguageName (German/French/Spanish)                 │ │
│ │ MasteryLevel                                         │ │
│ │ CreatedDate, LastModifiedDate                        │ │
│ │ IsActive                                             │ │
│ └──────────────────────┬───────────────────────────────┘ │
└───────────────────────┼────────────────────────────────────┘
         1-to-Many      │
    ┌────┴─────┬────────┴──────┬─────────┐
    │           │               │         │
    ▼           ▼               ▼         ▼
┌─────────┐ ┌────────┐ ┌────────────┐ ┌──────────┐
│Vocabulary│ │VerbEntry │ │LanguageItem│ │  (Other) │
│ (FK)    │ │ (FK)     │ │  (FK)      │ │          │
└─────────┘ └────────┘ └────────────┘ └──────────┘
```

---

## 🛡️ SECURITY CHECKLIST

Before deploying to production:

- ✅ User authentication implemented
- ✅ Password hashing (Identity handles this)
- ✅ HTTPS enabled in production
- ✅ CORS configured properly
- ✅ CSRF protection (Razor Pages default)
- ✅ Data isolation queries validated
- ✅ SQL injection prevention (EF Core parameterization)
- ✅ Authorization [Authorize] attributes in place
- ⏳ TODO: Add rate limiting for login attempts
- ⏳ TODO: Add 2FA (Two-Factor Authentication)
- ⏳ TODO: Add email confirmation before login

---

## 🐛 COMMON ISSUES & SOLUTIONS

### Issue: User can see other users' data
**Solution**: Add `.Where(x => x.LanguageProfileId == langProfileId)` to all queries

### Issue: Edit operation allows changing other users' data
**Solution**: Validate with `FirstOrDefaultAsync(x => x.Id == id && x.LanguageProfileId == langProfileId)`

### Issue: User can't add new items
**Solution**: Ensure `LanguageProfileId` is set before `_db.SaveChangesAsync()`

### Issue: Language profile not created on registration
**Solution**: Check Register.cshtml.cs - the code to create default profiles is there

### Issue: 403 Forbidden on Index page
**Solution**: Make sure user is logged in; check cookies

---

## 📋 DATABASE QUERIES REFERENCE

### Find All Users' Vocabulary
```csharp
var allVocab = await _db.Vocabulary
    .Include(v => v.LanguageProfile)
    .ThenInclude(lp => lp.UserProfile)
    .ToListAsync();
```

### Find User's Total Mastery Across All Languages
```csharp
var userId = 1;
var avgMastery = await _db.LanguageProfiles
    .Where(lp => lp.UserId == userId)
    .SelectMany(lp => lp.VocabularyItems)
    .AverageAsync(v => v.Mastery);
```

### Find All German Learners
```csharp
var germanLearners = await _db.LanguageProfiles
    .Where(lp => lp.LanguageCode == "de")
    .Include(lp => lp.UserProfile)
    .ToListAsync();
```

### Count User's Vocabulary by Language
```csharp
var userId = 1;
var stats = await _db.LanguageProfiles
    .Where(lp => lp.UserId == userId)
    .Select(lp => new 
    { 
        Language = lp.LanguageName,
        Count = lp.VocabularyItems.Count(),
        AvgMastery = lp.VocabularyItems.Average(v => v.Mastery)
    })
    .ToListAsync();
```

---

## 🔄 DATA MIGRATION NOTES

If migrating existing data:

```csharp
// 1. Create default user (one-time)
var defaultUser = new ApplicationUser { UserName = "admin@linguistpro.com" };
await userManager.CreateAsync(defaultUser, "Password123!");

// 2. Create user profile
var profile = new UserProfile { UserId = defaultUser.Id, FirstName = "Admin" };
_db.UserProfiles.Add(profile);
await _db.SaveChangesAsync();

// 3. Create default language profiles
var langProfiles = new[]
{
    new LanguageProfile { UserId = defaultUser.Id, LanguageCode = "de", LanguageName = "German" },
    new LanguageProfile { UserId = defaultUser.Id, LanguageCode = "fr", LanguageName = "French" }
};
_db.LanguageProfiles.AddRange(langProfiles);
await _db.SaveChangesAsync();

// 4. Update existing vocabulary (assuming it's German)
var germanProfile = langProfiles.First(l => l.LanguageCode == "de");
foreach (var vocab in _db.Vocabulary.Where(v => v.LanguageProfileId == null))
{
    vocab.LanguageProfileId = germanProfile.LanguageProfileId;
}
await _db.SaveChangesAsync();
```

---

## 🧪 TESTING CHECKLIST

### User Authentication
- [ ] Register with valid credentials
- [ ] Register fails with weak password
- [ ] Register fails with duplicate email
- [ ] Login with correct credentials
- [ ] Login fails with wrong password
- [ ] Logout clears session
- [ ] Access protected page without login → redirects to login

### Data Isolation
- [ ] Add vocabulary as User1
- [ ] Login as User2 → User2 sees empty vocab list
- [ ] Switch User2 to another language → still empty
- [ ] Login back as User1 → sees their vocabulary
- [ ] User2 can't edit User1's vocabulary by ID manipulation

### Language Profiles
- [ ] Register creates 3 default language profiles (de, fr, es)
- [ ] Can switch between languages
- [ ] Each language maintains separate vocabulary
- [ ] Adding item to one language doesn't appear in other

### CRUD Operations
- [ ] Add vocabulary ✓
- [ ] Edit own vocabulary ✓
- [ ] Delete own vocabulary ✓
- [ ] Try to edit other user's vocabulary → no change ✓
- [ ] Try to delete other user's vocabulary → no deletion ✓

---

## 🔧 DEVELOPMENT TIPS

### Enable SQL Logging
```csharp
// In Program.cs
optionsBuilder.LogTo(Console.WriteLine);
```

### Check Current User
```csharp
// In any PageModel
var user = await _userManager.GetUserAsync(User);
Console.WriteLine($"Current user: {user?.Email}");
```

### Debug Language Profile
```csharp
var langProfile = await GetCurrentLanguageProfile();
Console.WriteLine($"Language Profile ID: {langProfile?.LanguageProfileId}");
Console.WriteLine($"Language: {langProfile?.LanguageName}");
```

---

## 📚 RELATED FILES

- **Authentication Config**: `Program.cs`
- **Models**: `Models/` directory
- **Pages**: `Pages/Account/` and `Pages/Index.cshtml.cs`
- **Database**: `linguist.db`
- **Migrations**: `Migrations/` directory

---

**Last Updated**: February 7, 2025  
**Version**: 1.0 - Production Ready  
**.NET Version**: 8.0
