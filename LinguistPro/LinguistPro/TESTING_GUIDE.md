# Testing Guide - Number Interpretation & Sorting

## Compound Number Testing

### Test Cases for `getNumberValue()` function

```javascript
// Basic numbers
getNumberValue("zero")                           // Expected: 0
getNumberValue("five")                           // Expected: 5
getNumberValue("twenty")                         // Expected: 20
getNumberValue("ninety nine")                    // Expected: 99

// Hundred variants
getNumberValue("one hundred")                    // Expected: 100
getNumberValue("hundred")                        // Expected: 100
getNumberValue("two hundred")                    // Expected: 200
getNumberValue("nine hundred")                   // Expected: 900

// Thousand variants
getNumberValue("one thousand")                   // Expected: 1000
getNumberValue("thousand")                       // Expected: 1000

// COMPOUND NUMBERS (NEW FUNCTIONALITY)
getNumberValue("one hundred and five")           // Expected: 105
getNumberValue("one hundred and fifteen")        // Expected: 115
getNumberValue("two hundred and fifty six")      // Expected: 256
getNumberValue("five hundred and seventy two")   // Expected: 572
getNumberValue("one thousand and one")           // Expected: 1001
getNumberValue("one thousand two hundred and thirty four")  // Expected: 1234
getNumberValue("nine thousand nine hundred and ninety nine") // Expected: 9999

// Edge cases
getNumberValue("million")                        // Expected: 1000000
getNumberValue("one million")                    // Expected: 1000000
getNumberValue("")                               // Expected: null
getNumberValue(null)                             // Expected: null
getNumberValue("unknown")                        // Expected: null

// Case insensitive
getNumberValue("FIVE")                           // Expected: 5
getNumberValue("One Hundred AND Fifteen")        // Expected: 115
```

## Day Sorting Testing

### Manual Test Steps

1. **Navigate to Days section**
   - Click "📆 Days" in sidebar
   - Should see a list of days

2. **Test Initial Order**
   ```
   Expected order on page load:
   Monday
   Tuesday
   Wednesday
   Thursday
   Friday
   Saturday
   Sunday
   ```

3. **Test Add Operation**
   - Add new day entry (e.g., "Wednesday")
   - Check if new entry appears in correct position
   - Should re-sort automatically

4. **Test Edit Operation**
   - Edit a day entry
   - Modify and save
   - Should maintain correct position
   - Should re-sort after save

5. **Test Delete Operation**
   - Delete a day entry
   - Should re-sort remaining entries
   - List should maintain correct order

## Month Sorting Testing

### Manual Test Steps

1. **Navigate to Months section**
   - Click "📅 Months" in sidebar
   - Should see a list of months

2. **Test Initial Order**
   ```
   Expected order on page load:
   January
   February
   March
   April
   May
   June
   July
   August
   September
   October
   November
   December
   ```

3. **Test Add Operation**
   - Add new month entry (e.g., "March")
   - Check if new entry appears in correct position
   - Should re-sort automatically

4. **Test Edit Operation**
   - Edit a month entry
   - Modify and save
   - Should maintain correct position
   - Should re-sort after save

5. **Test Delete Operation**
   - Delete a month entry
   - Should re-sort remaining entries
   - List should maintain correct order

## Number Sorting Testing

### Manual Test Steps

1. **Navigate to Numbers section**
   - Click "🔢 Numbers" in sidebar
   - Should see a list of numbers with badges

2. **Test Initial Order**
   ```
   Expected: Numeric order
   0, 1, 2, 3, ..., 99, 100, 200, ..., 1000
   ```

3. **Test Number Badges**
   - Each card should show numeric value
   - "twenty six" card should display badge "26"
   - "one hundred and fifteen" should display "115"

4. **Test Add Compound Number**
   - Add "one hundred and twenty five"
   - Badge should show "125"
   - Card should appear between "124" and "126"

5. **Test Add Large Number**
   - Add "one thousand five hundred"
   - Badge should show "1500"
   - Card should appear after "1499"

6. **Test Edit Operation**
   - Edit "fifty" to "fifty one"
   - Badge should update from "50" to "51"
   - Card should move to new position

7. **Test Delete Operation**
   - Delete a number entry
   - Remaining entries should stay sorted

8. **Test Mixed Entry Types**
   - Add both simple and compound numbers
   - Should all sort correctly together
   - Example order: 5, 20, 25, 100, 115, 200, 256, 1000

## Browser Console Testing

```javascript
// Open browser DevTools Console (F12)

// Test number parsing
console.log(getNumberValue("one hundred and twenty five"));  // 125
console.log(getNumberValue("five hundred"));                // 500
console.log(parseCompoundNumber("one hundred and fifteen"));// 115

// Test sorting functions
console.log(getDayOrder("Monday"));      // 0
console.log(getDayOrder("Friday"));      // 4
console.log(getMonthOrder("January"));   // 0
console.log(getMonthOrder("December"));  // 11

// Test with data attributes
var badge = document.querySelector('[data-number-value]');
if (badge) {
    console.log("Badge value:", badge.getAttribute('data-number-value'));
    console.log("Parsed number:", getNumberValue(badge.getAttribute('data-number-value')));
}
```

## Automated Test Cases (Unit Testing)

```csharp
// These tests would go in a test project
[TestClass]
public class NumberMappingTests
{
    [TestMethod]
    public void GetNumberValue_BasicNumbers()
    {
        Assert.AreEqual(0, GetNumberValue("zero"));
        Assert.AreEqual(5, GetNumberValue("five"));
        Assert.AreEqual(99, GetNumberValue("ninety nine"));
    }

    [TestMethod]
    public void GetNumberValue_CompoundNumbers()
    {
        Assert.AreEqual(115, GetNumberValue("one hundred and fifteen"));
        Assert.AreEqual(256, GetNumberValue("two hundred and fifty six"));
        Assert.AreEqual(1234, GetNumberValue("one thousand two hundred and thirty four"));
    }

    [TestMethod]
    public void GetNumberValue_CaseInsensitive()
    {
        Assert.AreEqual(GetNumberValue("five"), GetNumberValue("FIVE"));
        Assert.AreEqual(GetNumberValue("one hundred"), GetNumberValue("ONE HUNDRED"));
    }

    [TestMethod]
    public void GetNumberValue_InvalidInput()
    {
        Assert.IsNull(GetNumberValue(""));
        Assert.IsNull(GetNumberValue(null));
        Assert.IsNull(GetNumberValue("invalid"));
    }
}
```

## Refresh Testing

### Test Scenarios

1. **Page Load Refresh**
   - Close tab, reopen page
   - All numbers/days/months should be sorted correctly
   - ✅ Should work

2. **Add → Return → Check Sorting**
   - Add new item
   - Verify modal closes and page reloads
   - Item should be in correct sorted position
   - ✅ Should work with `pageshow` event

3. **Edit → Return → Check Sorting**
   - Edit an item
   - Change its meaning/term
   - Save and return
   - Item should re-sort to new position
   - ✅ Should work with `pageshow` event

4. **Delete → Check Sorting**
   - Delete an item
   - Remaining items should maintain sort order
   - ✅ Should work with `pageshow` event

5. **Multiple Operations**
   - Add item
   - Edit same item
   - Delete another item
   - All operations should maintain sort order
   - ✅ Should work

6. **Rapid Operations**
   - Add multiple items quickly
   - Edit several items
   - All should be sorted correctly
   - ✅ Should work (pageshow waits for navigation)

## Performance Testing

```javascript
// Test sorting performance with many items
console.time("sortNumberCards");
sortNumberCards();
console.timeEnd("sortNumberCards");
// Expected: < 100ms for 100+ items
```

## Known Limitations & Edge Cases

1. **Not Handled**:
   - Numbers beyond million (currently supported: up to 1,000,000)
   - Hyphenated numbers in some formats (e.g., "twenty-five")
   - British vs American number naming differences

2. **Behavior**:
   - Unknown/invalid number formats return `null`
   - Sorting with nulls places them at end (999 position)
   - Case-insensitive matching for all strings

3. **Performance**:
   - DOM reordering O(n) where n = number of items
   - Expected: Imperceptible for < 1000 items
   - Good practice: Index database queries by language profile

## Regression Testing Checklist

After making changes, verify:
- [ ] Numbers still sort by value
- [ ] Days still appear in calendar order
- [ ] Months still appear in calendar order
- [ ] Compound numbers parse correctly
- [ ] Add operations trigger sort
- [ ] Edit operations trigger sort
- [ ] Delete operations trigger sort
- [ ] Page refresh triggers sort
- [ ] No JavaScript console errors
- [ ] Performance acceptable (< 100ms for sort)

---

**Last Updated**: Today
**Test Status**: Ready for manual & automated testing
**Browser Support**: Chrome, Firefox, Safari, Edge
