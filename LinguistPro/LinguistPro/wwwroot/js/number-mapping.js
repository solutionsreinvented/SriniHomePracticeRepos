// Comprehensive English number to numeric value mapping
// Ordered by numeric value for proper sorting
const NUMBER_MAP = [
    { text: "zero", value: 0 },
    { text: "one", value: 1 },
    { text: "two", value: 2 },
    { text: "three", value: 3 },
    { text: "four", value: 4 },
    { text: "five", value: 5 },
    { text: "six", value: 6 },
    { text: "seven", value: 7 },
    { text: "eight", value: 8 },
    { text: "nine", value: 9 },
    { text: "ten", value: 10 },
    { text: "eleven", value: 11 },
    { text: "twelve", value: 12 },
    { text: "thirteen", value: 13 },
    { text: "fourteen", value: 14 },
    { text: "fifteen", value: 15 },
    { text: "sixteen", value: 16 },
    { text: "seventeen", value: 17 },
    { text: "eighteen", value: 18 },
    { text: "nineteen", value: 19 },
    { text: "twenty", value: 20 },
    { text: "twenty one", value: 21 },
    { text: "twenty two", value: 22 },
    { text: "twenty three", value: 23 },
    { text: "twenty four", value: 24 },
    { text: "twenty five", value: 25 },
    { text: "twenty six", value: 26 },
    { text: "twenty seven", value: 27 },
    { text: "twenty eight", value: 28 },
    { text: "twenty nine", value: 29 },
    { text: "thirty", value: 30 },
    { text: "thirty one", value: 31 },
    { text: "thirty two", value: 32 },
    { text: "thirty three", value: 33 },
    { text: "thirty four", value: 34 },
    { text: "thirty five", value: 35 },
    { text: "thirty six", value: 36 },
    { text: "thirty seven", value: 37 },
    { text: "thirty eight", value: 38 },
    { text: "thirty nine", value: 39 },
    { text: "forty", value: 40 },
    { text: "forty one", value: 41 },
    { text: "forty two", value: 42 },
    { text: "forty three", value: 43 },
    { text: "forty four", value: 44 },
    { text: "forty five", value: 45 },
    { text: "forty six", value: 46 },
    { text: "forty seven", value: 47 },
    { text: "forty eight", value: 48 },
    { text: "forty nine", value: 49 },
    { text: "fifty", value: 50 },
    { text: "fifty one", value: 51 },
    { text: "fifty two", value: 52 },
    { text: "fifty three", value: 53 },
    { text: "fifty four", value: 54 },
    { text: "fifty five", value: 55 },
    { text: "fifty six", value: 56 },
    { text: "fifty seven", value: 57 },
    { text: "fifty eight", value: 58 },
    { text: "fifty nine", value: 59 },
    { text: "sixty", value: 60 },
    { text: "sixty one", value: 61 },
    { text: "sixty two", value: 62 },
    { text: "sixty three", value: 63 },
    { text: "sixty four", value: 64 },
    { text: "sixty five", value: 65 },
    { text: "sixty six", value: 66 },
    { text: "sixty seven", value: 67 },
    { text: "sixty eight", value: 68 },
    { text: "sixty nine", value: 69 },
    { text: "seventy", value: 70 },
    { text: "seventy one", value: 71 },
    { text: "seventy two", value: 72 },
    { text: "seventy three", value: 73 },
    { text: "seventy four", value: 74 },
    { text: "seventy five", value: 75 },
    { text: "seventy six", value: 76 },
    { text: "seventy seven", value: 77 },
    { text: "seventy eight", value: 78 },
    { text: "seventy nine", value: 79 },
    { text: "eighty", value: 80 },
    { text: "eighty one", value: 81 },
    { text: "eighty two", value: 82 },
    { text: "eighty three", value: 83 },
    { text: "eighty four", value: 84 },
    { text: "eighty five", value: 85 },
    { text: "eighty six", value: 86 },
    { text: "eighty seven", value: 87 },
    { text: "eighty eight", value: 88 },
    { text: "eighty nine", value: 89 },
    { text: "ninety", value: 90 },
    { text: "ninety one", value: 91 },
    { text: "ninety two", value: 92 },
    { text: "ninety three", value: 93 },
    { text: "ninety four", value: 94 },
    { text: "ninety five", value: 95 },
    { text: "ninety six", value: 96 },
    { text: "ninety seven", value: 97 },
    { text: "ninety eight", value: 98 },
    { text: "ninety nine", value: 99 },
    { text: "one hundred", value: 100 },
    { text: "hundred", value: 100 },
    { text: "two hundred", value: 200 },
    { text: "three hundred", value: 300 },
    { text: "four hundred", value: 400 },
    { text: "five hundred", value: 500 },
    { text: "six hundred", value: 600 },
    { text: "seven hundred", value: 700 },
    { text: "eight hundred", value: 800 },
    { text: "nine hundred", value: 900 },
    { text: "one thousand", value: 1000 },
    { text: "thousand", value: 1000 },
    { text: "one million", value: 1000000 },
    { text: "million", value: 1000000 }
];

// Days of week in proper order
const DAYS_ORDER = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

// Months in proper order
const MONTHS_ORDER = ["January", "February", "March", "April", "May", "June", 
                      "July", "August", "September", "October", "November", "December"];

// Function to get numeric value from English number text (supports compound numbers like "one hundred and fifteen")
function getNumberValue(englishText) {
    if (!englishText) return null;
    const normalizedText = englishText.toLowerCase().trim();

    // Check direct match first
    const entry = NUMBER_MAP.find(item => item.text === normalizedText);
    if (entry) return entry.value;

    // Handle compound numbers like "one hundred and fifteen", "two thousand five hundred"
    return parseCompoundNumber(normalizedText);
}

// Parse compound numbers like "one hundred and fifteen" -> 115
function parseCompoundNumber(text) {
    const parts = text.split(/\s+and\s+/);
    let total = 0;
    let currentValue = 0;

    for (const part of parts) {
        const words = part.split(/\s+/);

        for (const word of words) {
            const entry = NUMBER_MAP.find(item => item.text === word);
            if (entry) {
                if (entry.value >= 100) {
                    currentValue = (currentValue || 1) * entry.value;
                } else {
                    currentValue += entry.value;
                }
            }
        }
    }

    total += currentValue;
    return total > 0 ? total : null;
}

// Function to get sort order index for Days
function getDayOrder(dayName) {
    const index = DAYS_ORDER.findIndex(day => day.toLowerCase() === (dayName || '').toLowerCase());
    return index >= 0 ? index : 999;
}

// Function to get sort order index for Months
function getMonthOrder(monthName) {
    const index = MONTHS_ORDER.findIndex(month => month.toLowerCase() === (monthName || '').toLowerCase());
    return index >= 0 ? index : 999;
}

// Function to sort number cards by numeric value
function sortNumberCards() {
    const container = document.querySelector('.flex.flex-wrap.gap-4');
    if (!container) return;

    // Get all vocab cards that have a number badge
    const cards = Array.from(container.querySelectorAll('.vocab-card'));
    const cardsWithBadges = cards.filter(card => {
        return card.querySelector('[data-number-value]') !== null;
    });

    // Sort cards by numeric value
    cardsWithBadges.sort((aCard, bCard) => {
        const aBadge = aCard.querySelector('[data-number-value]');
        const bBadge = bCard.querySelector('[data-number-value]');

        const aValue = getNumberValue(aBadge.getAttribute('data-number-value'));
        const bValue = getNumberValue(bBadge.getAttribute('data-number-value'));

        if (aValue === null || bValue === null) return 0;
        return aValue - bValue;
    });

    // Reorder cards in the DOM
    cardsWithBadges.forEach(card => {
        container.appendChild(card);
    });
}

// Function to sort day cards by day order
function sortDayCards() {
    const container = document.querySelector('.flex.flex-wrap.gap-4');
    if (!container) return;

    const cards = Array.from(container.querySelectorAll('.vocab-card'));

    // Sort cards by day order
    cards.sort((aCard, bCard) => {
        const aTerm = aCard.dataset.itemTerm || '';
        const bTerm = bCard.dataset.itemTerm || '';

        const aOrder = getDayOrder(aTerm);
        const bOrder = getDayOrder(bTerm);

        return aOrder - bOrder;
    });

    // Reorder cards in the DOM
    cards.forEach(card => {
        container.appendChild(card);
    });
}

// Function to sort month cards by month order
function sortMonthCards() {
    const container = document.querySelector('.flex.flex-wrap.gap-4');
    if (!container) return;

    const cards = Array.from(container.querySelectorAll('.vocab-card'));

    // Sort cards by month order
    cards.sort((aCard, bCard) => {
        const aTerm = aCard.dataset.itemTerm || '';
        const bTerm = bCard.dataset.itemTerm || '';

        const aOrder = getMonthOrder(aTerm);
        const bOrder = getMonthOrder(bTerm);

        return aOrder - bOrder;
    });

    // Reorder cards in the DOM
    cards.forEach(card => {
        container.appendChild(card);
    });
}
