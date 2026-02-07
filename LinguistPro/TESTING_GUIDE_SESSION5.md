# 🧪 **QUICK TESTING GUIDE - SESSION 5**

## ✅ **BUILD STATUS**
```
✅ Build Successful - Ready to Test
```

---

## 🧪 **TEST 1: ManageAdmins Exception Fix**

**Scenario:** AdminUsers table doesn't exist (no migrations run)

**Steps:**
1. Visit `/Admin/ManageAdmins`
2. You should see error message (NOT a crash):
   ```
   ⚠️ Admin database table not initialized. 
   Run migrations first: 
   dotnet ef migrations add AddAdminUsers && dotnet ef database update
   ```

**Expected:** ✅ Graceful error message (not 500 error)

---

## 🧪 **TEST 2: Bulk Delete - Checkbox Wiring**

**Prerequisites:** Must be logged in

**Steps:**
1. Go to `/Index` (or `/Index?mode=Vocab`)
2. Look for checkboxes next to vocabulary items
3. **Check one checkbox** ✓
   - "Delete Selected" button should **ENABLE** (not grayed out)
   - Counter should show "1 selected"
4. **Check multiple checkboxes** ✓
   - Counter updates (e.g., "3 selected")
   - Button stays enabled
5. **Click "Select All"** ✓
   - All checkboxes check
   - Counter shows total count
6. **Uncheck one** ✓
   - Counter decreases
   - Button still enabled
7. **Uncheck all** ✓
   - Button **DISABLES** (grayed out)
   - Counter shows "0 selected"

**Expected Result:**
```
Checkboxes checked: 0 → Button DISABLED ✅
Checkboxes checked: 1+ → Button ENABLED ✅
```

---

## 🧪 **TEST 3: Admin-Only Access Control**

### **Test 3A: Regular User (No Admin Session)**

**Setup:** Login as regular user (NOT admin)

**Steps:**
1. Go to `/Index`
2. Check a vocabulary item checkbox
3. Click "Delete Selected" button
4. Should get **401 Unauthorized** response

**Expected:** ✅ Cannot delete (access denied)

### **Test 3B: Admin User (With Admin Session)**

**Setup:** Login to `/Admin/Login` with admin/admin123

**Steps:**
1. Go to `/Index`
2. Check vocabulary items
3. Click "Delete Selected"
4. Confirm dialog
5. Items should delete successfully

**Expected:** ✅ Can delete multiple items

---

## 🧪 **TEST 4: Verify Days/Months/Numbers Categorization**

**Prerequisites:** Run migrations and fetch new data

```bash
cd LinguistPro
dotnet ef migrations add AddAdminUsers
dotnet ef database update
dotnet run
```

**Steps:**
1. Go to `/Admin/AutoPopulate`
2. Login if needed
3. Select **German**
4. Category: **Days**
5. Click "Start Fetching"
6. Wait for completion

**Verification:**
1. Go back to `/Index`
2. Click on "📆 Days" in sidebar
3. Should show days **NOT** in vocabulary section
4. Should show in **Days section** only

**Expected:** ✅ Days in Days section, NOT in Vocabulary

**Repeat for:**
- Months (📅 Months)
- Numbers (🔢 Numbers)

---

## 🧪 **TEST 5: Complete Bulk Delete Workflow**

**Setup:** Login as admin

**Steps:**
1. Go to `/Index?mode=Vocab`
2. Note current vocabulary count
3. Select 2-3 items with checkboxes
4. Note count (e.g., "3 selected")
5. Click "🗑️ Delete Selected"
6. Dialog asks: "Delete 3 vocabulary items? This cannot be undone."
7. Click "Delete" button
8. Wait for redirect

**Verification:**
1. Page reloads
2. Selected items gone
3. Vocabulary count decreased by 3

**Expected:** ✅ Bulk delete works smoothly

---

## 🧪 **TEST 6: ManageAdmins (After Migrations)**

**Prerequisites:** Run migrations first

```bash
dotnet ef migrations add AddAdminUsers
dotnet ef database update
```

**Steps:**
1. Login to `/Admin/Login` as admin
2. Go to `/Admin/ManageAdmins`
3. Should see list of admin users
4. Click "Create new admin user"
5. Fill:
   - Username: testadmin
   - Password: testpass123
   - Confirm: testpass123
   - Full Name: Test Admin (optional)
6. Click "Create Admin User"
7. Should see success message
8. New admin appears in list

**Verification:**
1. Logout
2. Try login with testadmin / testpass123
3. Should succeed ✅

**Delete Test:**
1. In ManageAdmins, click 🗑️ Delete on admin user
2. Should ask confirmation
3. After delete, user gone from list

**Expected:** ✅ Admin management works

---

## ⚠️ **TROUBLESHOOTING**

### **Problem: Bulk delete button stays disabled**
**Solution:** 
- Refresh page
- Clear browser cache
- Verify bulk-delete.js is loaded (check Network tab)

### **Problem: "Delete Selected" button not visible**
**Solution:**
- Must be in edit mode (checkbox visible)
- Refresh page
- Try different browser

### **Problem: ManageAdmins still shows exception**
**Solution:**
- Run migrations: `dotnet ef migrations add AddAdminUsers`
- Update database: `dotnet ef database update`
- Rebuild: `dotnet build`

### **Problem: Days showing in Vocabulary**
**Solution:**
- Old data in database
- Run: `DELETE FROM LanguageItems WHERE ItemType='Day'`
- Re-fetch Days from AutoPopulate

---

## 📋 **FINAL CHECKLIST**

After testing, verify all passing:

- [ ] ManageAdmins doesn't crash
- [ ] Bulk delete buttons wire to checkboxes
- [ ] Delete disabled when nothing selected
- [ ] Delete enabled when items selected
- [ ] Admin auth working
- [ ] Regular users blocked from delete
- [ ] Admin users can delete
- [ ] Days/Months/Numbers in correct sections
- [ ] ManageAdmins create/delete works
- [ ] Admin login works with new credentials

---

## 🚀 **READY FOR PRODUCTION**

Once all tests pass:

```bash
# Build final
dotnet build

# Run
dotnet run

# Deploy to production
```

✅ **ALL TESTS PASSING = PRODUCTION READY**

