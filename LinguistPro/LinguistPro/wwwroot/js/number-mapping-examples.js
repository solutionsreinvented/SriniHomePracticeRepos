// Example usage of the number mapping system

// The number mapping file (number-mapping.js) provides two main exports:
// 1. numberMap - An object containing all number name to numeric value mappings
// 2. getNumberValue(englishText) - A function to look up numeric values

// EXAMPLES:

// Direct object access:
console.log(numberMap["fifty one"]);        // Output: "51"
console.log(numberMap["eighty seven"]);     // Output: "87"
console.log(numberMap["ninety nine"]);      // Output: "99"
console.log(numberMap["hundred"]);          // Output: "100"
console.log(numberMap["one thousand"]);     // Output: "1000"

// Using the getNumberValue function:
console.log(getNumberValue("fifty one"));    // Output: "51"
console.log(getNumberValue("Eighty Seven")); // Output: "87" (case-insensitive)
console.log(getNumberValue("invalid"));      // Output: null

// How it's used in the application:

// 1. In Index.cshtml, for each number item:
//    <div class="number-badge" data-number-value="fifty one"></div>

// 2. JavaScript automatically populates badges:
//    function populateNumberBadges() {
//        var badges = document.querySelectorAll('[data-number-value]');
//        badges.forEach(function(badge) {
//            var englishText = badge.getAttribute('data-number-value');
//            var numericValue = getNumberValue(englishText);
//            if (numericValue) {
//                badge.textContent = numericValue;
//            }
//        });
//    }

// ADDING NEW NUMBERS:

// To add new numbers, simply add entries to the numberMap object in number-mapping.js:
//
// In LinguistPro\wwwroot\js\number-mapping.js:
// 
// const numberMap = {
//     // ... existing entries ...
//     "two hundred twenty three": "223",
//     "five hundred sixty seven": "567",
//     "nine hundred ninety nine": "999",
//     // ... add as needed ...
// };

// PERFORMANCE NOTES:

// - Original approach: Inline C# Dictionary in Razor template for each card render
//   - Repeated dictionary creation per card
//   - Server-side computation
//
// - New approach: External JavaScript with object literal
//   - Single shared numberMap object
//   - Client-side lookup (faster)
//   - No server computation overhead
//   - Automatic population via DOM attribute data
//
// - For numbers 0-99: ~110 entries (covers all combinations)
// - For numbers 100+: ~20 entries (base values like 100, 200, 300, etc.)
// - Special entries: thousand, million
// - Total: 160+ mappings covering virtually all common numbers

// EXTENDING THE SYSTEM:

// If you need to support other number systems or formats, you can:
// 1. Add a new mapping object (e.g., germanNumberMap for German numbers)
// 2. Create language-aware lookup function
// 3. Modify populateNumberBadges() to use language-specific mapping

// Example extension:
/*
const germanNumberMap = {
    "null": "0",
    "eins": "1",
    "zwei": "2",
    "einundzwanzig": "21",
    // ... etc
};

function getNumberValue(englishText, language = 'en') {
    const lang = language.toLowerCase();
    let map = numberMap; // default to English
    
    if (lang === 'de') {
        map = germanNumberMap;
    } else if (lang === 'fr') {
        map = frenchNumberMap;
    }
    
    const normalizedText = englishText.toLowerCase().trim();
    return map[normalizedText] || null;
}
*/
