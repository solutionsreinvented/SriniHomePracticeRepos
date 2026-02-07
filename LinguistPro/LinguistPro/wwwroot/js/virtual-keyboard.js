var currentInputField = null;
var selectedLanguage = 'de';
var capsLockActive = false;
var shiftKeyActive = false;

// Keyboard layouts for different languages - WITH NUMBERS AND SYMBOLS
var keyboardLayouts = {
    de: {
        name: 'German',
        rows: [
            [
                { key: '1', shift: '!' },
                { key: '2', shift: '@' },
                { key: '3', shift: '#' },
                { key: '4', shift: '$' },
                { key: '5', shift: '%' },
                { key: '6', shift: '^' },
                { key: '7', shift: '&' },
                { key: '8', shift: '*' },
                { key: '9', shift: '(' },
                { key: '0', shift: ')' }
            ],
            [
                { key: 'q' },
                { key: 'w' },
                { key: 'e' },
                { key: 'r' },
                { key: 't' },
                { key: 'z' },
                { key: 'u' },
                { key: 'i' },
                { key: 'o' },
                { key: 'p' },
                { key: 'ü' }
            ],
            [
                { key: 'a' },
                { key: 's' },
                { key: 'd' },
                { key: 'f' },
                { key: 'g' },
                { key: 'h' },
                { key: 'j' },
                { key: 'k' },
                { key: 'l' },
                { key: 'ö' },
                { key: 'ä' }
            ],
            [
                { key: 'y' },
                { key: 'x' },
                { key: 'c' },
                { key: 'v' },
                { key: 'b' },
                { key: 'n' },
                { key: 'm' },
                { key: ',', shift: '<' },
                { key: '.', shift: '>' },
                { key: 'ß', shift: '?' }
            ]
        ],
        symbols: [
            { key: '`', shift: '~' },
            { key: '-', shift: '_' },
            { key: '=', shift: '+' },
            { key: '[', shift: '{' },
            { key: ']', shift: '}' },
            { key: '\\', shift: '|' },
            { key: ';', shift: ':' },
            { key: "'", shift: '"' },
            { key: '/', shift: '?' }
        ]
    },
    fr: {
        name: 'French',
        rows: [
            [
                { key: '1', shift: '&' },
                { key: '2', shift: 'é' },
                { key: '3', shift: '"' },
                { key: '4', shift: "'" },
                { key: '5', shift: '(' },
                { key: '6', shift: '-' },
                { key: '7', shift: 'è' },
                { key: '8', shift: '_' },
                { key: '9', shift: 'ç' },
                { key: '0', shift: 'à' }
            ],
            [
                { key: 'a' },
                { key: 'z' },
                { key: 'e' },
                { key: 'r' },
                { key: 't' },
                { key: 'y' },
                { key: 'u' },
                { key: 'i' },
                { key: 'o' },
                { key: 'p' }
            ],
            [
                { key: 'q' },
                { key: 's' },
                { key: 'd' },
                { key: 'f' },
                { key: 'g' },
                { key: 'h' },
                { key: 'j' },
                { key: 'k' },
                { key: 'l' },
                { key: 'm' }
            ],
            [
                { key: 'w' },
                { key: 'x' },
                { key: 'c' },
                { key: 'v' },
                { key: 'b' },
                { key: 'n' },
                { key: ',', shift: '?' },
                { key: '.', shift: '/' }
            ]
        ],
        symbols: [
            { key: '=', shift: '+' },
            { key: '[', shift: '{' },
            { key: ']', shift: '}' },
            { key: '\\', shift: '|' },
            { key: ';', shift: '.' }
        ]
    },
    es: {
        name: 'Spanish',
        rows: [
            [
                { key: '1', shift: '!' },
                { key: '2', shift: '"' },
                { key: '3', shift: '#' },
                { key: '4', shift: '$' },
                { key: '5', shift: '%' },
                { key: '6', shift: '&' },
                { key: '7', shift: '/' },
                { key: '8', shift: '(' },
                { key: '9', shift: ')' },
                { key: '0', shift: '=' }
            ],
            [
                { key: 'q' },
                { key: 'w' },
                { key: 'e' },
                { key: 'r' },
                { key: 't' },
                { key: 'y' },
                { key: 'u' },
                { key: 'i' },
                { key: 'o' },
                { key: 'p' }
            ],
            [
                { key: 'a' },
                { key: 's' },
                { key: 'd' },
                { key: 'f' },
                { key: 'g' },
                { key: 'h' },
                { key: 'j' },
                { key: 'k' },
                { key: 'l' },
                { key: 'ñ' }
            ],
            [
                { key: 'z' },
                { key: 'x' },
                { key: 'c' },
                { key: 'v' },
                { key: 'b' },
                { key: 'n' },
                { key: 'm' },
                { key: ',', shift: ';' },
                { key: '.', shift: ':' }
            ]
        ],
        symbols: [
            { key: '-', shift: '_' },
                { key: '`', shift: '^' },
            { key: '[', shift: '{' },
            { key: ']', shift: '}' },
            { key: '\\', shift: '|' }
        ]
    },
    english: {
        name: 'English',
        rows: [
            [
                { key: '1', shift: '!' },
                { key: '2', shift: '@' },
                { key: '3', shift: '#' },
                { key: '4', shift: '$' },
                { key: '5', shift: '%' },
                { key: '6', shift: '^' },
                { key: '7', shift: '&' },
                { key: '8', shift: '*' },
                { key: '9', shift: '(' },
                { key: '0', shift: ')' }
            ],
            [
                { key: 'q' },
                { key: 'w' },
                { key: 'e' },
                { key: 'r' },
                { key: 't' },
                { key: 'y' },
                { key: 'u' },
                { key: 'i' },
                { key: 'o' },
                { key: 'p' }
            ],
            [
                { key: 'a' },
                { key: 's' },
                { key: 'd' },
                { key: 'f' },
                { key: 'g' },
                { key: 'h' },
                { key: 'j' },
                { key: 'k' },
                { key: 'l' }
            ],
            [
                { key: 'z' },
                { key: 'x' },
                { key: 'c' },
                { key: 'v' },
                { key: 'b' },
                { key: 'n' },
                { key: 'm' },
                { key: ',', shift: '<' },
                { key: '.', shift: '>' }
            ]
        ],
        symbols: [
            { key: '-', shift: '_' },
            { key: '=', shift: '+' },
            { key: '[', shift: '{' },
            { key: ']', shift: '}' },
            { key: '\\', shift: '|' },
            { key: ';', shift: ':' },
            { key: "'", shift: '"' },
            { key: '/', shift: '?' }
        ]
    }
};

function openKeyboard(ev, element) {
    // element can be either the keyboard icon element or a direct input/textarea element
    if (ev && typeof ev.stopPropagation === 'function') ev.stopPropagation();

    if (!element) {
        // try to use activeElement
        var ae = document.activeElement;
        if (ae && (ae.tagName === 'INPUT' || ae.tagName === 'TEXTAREA')) {
            currentInputField = ae;
        }
    } else if (element.tagName === 'INPUT' || element.tagName === 'TEXTAREA') {
        currentInputField = element;
    } else {
        var parent = element.closest('div');
        currentInputField = parent ? parent.querySelector('input, textarea') : null;
    }

    if (!currentInputField) return;

    var selector = document.getElementById('keyboardLangSelector');
    selector.value = 'target';
    capsLockActive = false;
    shiftKeyActive = false;

    renderKeyboard();
    var modal = document.getElementById('keyboardModal');
    modal.classList.remove('hidden');
    modal.classList.add('flex');
    currentInputField.focus();
}

function closeKeyboard() {
    var modal = document.getElementById('keyboardModal');
    modal.classList.remove('flex');
    modal.classList.add('hidden');
    currentInputField = null;
    shiftKeyActive = false;
}

function renderKeyboard() {
    var selector = document.getElementById('keyboardLangSelector');
    var mode = selector.value;
    var lang = mode === 'target' ? (selectedLanguage || 'de') : 'english';
    var layout = keyboardLayouts[lang];

    if (!layout) layout = keyboardLayouts['de'];

    var container = document.getElementById('keyboardKeys');
    container.innerHTML = '';

    // Create main column for entire keyboard
    var mainCol = document.createElement('div');
    mainCol.className = 'keyboard-main';

    // Number row: display numbers with shift symbols in top-right corner
    if (layout.rows && layout.rows.length > 0) {
        var numRow = document.createElement('div');
        numRow.className = 'keyboard-row';
        layout.rows[0].forEach(function(keyObj) {
            var keyEl = document.createElement('div');
            keyEl.className = 'key key-num';
            keyEl.id = 'vkey_' + keyObj.key;

            // Main number display
            var numSpan = document.createElement('span');
            numSpan.textContent = keyObj.key;
            keyEl.appendChild(numSpan);

            // Shift symbol in top-right corner
            if (keyObj.shift) {
                var shiftSpan = document.createElement('span');
                shiftSpan.className = 'shift-symbol';
                shiftSpan.textContent = keyObj.shift;
                keyEl.appendChild(shiftSpan);
            }

            keyEl.title = keyObj.key + (keyObj.shift ? ' (Shift: ' + keyObj.shift + ')' : '');
            keyEl.onclick = function() { insertCharacter(keyObj); };
            numRow.appendChild(keyEl);
        });
        mainCol.appendChild(numRow);
    }

    // Main keyboard: letter rows (skip first row which is numbers)
    for (var r = 1; r < layout.rows.length; r++) {
        var row = layout.rows[r];
        var rowDiv = document.createElement('div');
        rowDiv.className = 'keyboard-row';
        row.forEach(function(keyObj) {
            var keyEl = document.createElement('div');
            keyEl.className = 'key key-small';
            keyEl.id = 'vkey_' + keyObj.key;
            var displayKey = getDisplayKey(keyObj);
            keyEl.textContent = displayKey;
            keyEl.title = keyObj.key + (keyObj.shift ? ' (Shift: ' + keyObj.shift + ')' : '');
            keyEl.onclick = function() { insertCharacter(keyObj); };
            rowDiv.appendChild(keyEl);
        });
        mainCol.appendChild(rowDiv);
    }

    // Add space bar
    var spaceKey = document.createElement('div');
    spaceKey.className = 'key space';
    spaceKey.id = 'vkey_space';
    spaceKey.textContent = 'Space';
    spaceKey.onclick = function() { insertCharacter({ key: ' ' }); };
    mainCol.appendChild(spaceKey);

    // Add control buttons row
    var controlsDiv = document.createElement('div');
    controlsDiv.style.display = 'flex';
    controlsDiv.style.gap = '4px';
    controlsDiv.style.justifyContent = 'flex-start';
    controlsDiv.style.marginTop = '8px';

    // Shift key
    var shiftKey = document.createElement('div');
    shiftKey.className = 'key shift' + (shiftKeyActive ? ' active' : '');
    shiftKey.id = 'vkey_shift';
    shiftKey.textContent = '⇧ Shift';
    shiftKey.onclick = function() { toggleShift(); };
    controlsDiv.appendChild(shiftKey);

    // Caps lock button
    var capsKey = document.createElement('div');
    capsKey.className = 'key caps' + (capsLockActive ? ' active' : '');
    capsKey.id = 'vkey_caps';
    capsKey.textContent = 'CAPS';
    capsKey.onclick = function() { toggleCapsLock(); };
    controlsDiv.appendChild(capsKey);

    // Backspace
    var backspaceKey = document.createElement('div');
    backspaceKey.className = 'key backspace';
    backspaceKey.id = 'vkey_backspace';
    backspaceKey.textContent = '← Backspace';
    backspaceKey.onclick = function() { deleteCharacter(); };
    controlsDiv.appendChild(backspaceKey);

    // Close button
    var closeKey = document.createElement('div');
    closeKey.className = 'key close';
    closeKey.textContent = 'Close (Esc)';
    closeKey.onclick = function() { closeKeyboard(); };
    controlsDiv.appendChild(closeKey);

    mainCol.appendChild(controlsDiv);

    // Add help info at the bottom
    addKeyboardInfo(mainCol);

    container.appendChild(mainCol);
}

function getDisplayKey(keyObj) {
    var baseKey = keyObj.key;

    // If shift is active and there's no shift property (letter key), show uppercase
    if (shiftKeyActive && baseKey && baseKey.length === 1 && baseKey.toLowerCase() !== baseKey.toUpperCase()) {
        return baseKey.toUpperCase();
    }

    // If shift is active and there IS a shift property, show the shift character
    if (shiftKeyActive && keyObj.shift) {
        return keyObj.shift;
    }

    // If caps is active and it's a letter, show uppercase
    if (capsLockActive && baseKey && baseKey.length === 1 && baseKey.toLowerCase() !== baseKey.toUpperCase()) {
        return baseKey.toUpperCase();
    }

    return baseKey;
}

function toggleCapsLock() {
    capsLockActive = !capsLockActive;
    shiftKeyActive = false;
    renderKeyboard();
}

function toggleShift() {
    shiftKeyActive = !shiftKeyActive;
    renderKeyboard();
}

function insertCharacter(keyObj, isPhysicalShift) {
    if (!currentInputField) return;

    var charToInsert = keyObj.key;
    var effectiveShift = shiftKeyActive || isPhysicalShift;

    // Handle shift + special characters
    if (effectiveShift && keyObj.shift) {
        charToInsert = keyObj.shift;
    } else if ((capsLockActive || effectiveShift) && keyObj.key && keyObj.key.length === 1 && keyObj.key.toLowerCase() !== keyObj.key.toUpperCase()) {
        // Unicode-aware letter detection
        charToInsert = keyObj.key.toUpperCase();
    }

    var start = currentInputField.selectionStart;
    var end = currentInputField.selectionEnd;
    var text = currentInputField.value;
    var newText = text.substring(0, start) + charToInsert + text.substring(end);

    currentInputField.value = newText;
    currentInputField.selectionStart = currentInputField.selectionEnd = start + charToInsert.length;
    currentInputField.focus();

    // Reset shift if it was active
    if (shiftKeyActive) {
        shiftKeyActive = false;
        renderKeyboard();
    }

    var event = new Event('input', { bubbles: true });
    currentInputField.dispatchEvent(event);
}

function deleteCharacter() {
    if (!currentInputField) return;

    var start = currentInputField.selectionStart;
    var end = currentInputField.selectionEnd;
    var text = currentInputField.value;

    if (start === end && start > 0) {
        var newText = text.substring(0, start - 1) + text.substring(start);
        currentInputField.value = newText;
        currentInputField.selectionStart = currentInputField.selectionEnd = start - 1;
    } else if (start !== end) {
        var newText = text.substring(0, start) + text.substring(end);
        currentInputField.value = newText;
        currentInputField.selectionStart = currentInputField.selectionEnd = start;
    }

    currentInputField.focus();

    var event = new Event('input', { bubbles: true });
    currentInputField.dispatchEvent(event);
}

// Tips removed - symbols are shown inline on number keys now

function addKeyboardInfo(targetContainer) {
    var container = targetContainer || document.getElementById('keyboardKeys');
    var infoDiv = document.createElement('div');
    infoDiv.className = 'keyboard-info';
    infoDiv.innerHTML = `
        <div class="keyboard-info-item">
            <span class="keyboard-info-key">Shift</span>
            <span>+ Key for alternate character</span>
        </div>
        <div class="keyboard-info-item">
            <span class="keyboard-info-key">Esc</span>
            <span>to close keyboard</span>
        </div>
        <div class="keyboard-info-item">
            <span class="keyboard-info-key">Shift</span>
            <span>+ Backspace = delete word</span>
        </div>
    `;
    container.appendChild(infoDiv);
}

// Handle keyboard language selector change
document.addEventListener('DOMContentLoaded', function() {
    var selector = document.getElementById('keyboardLangSelector');
    if (selector) {
        selector.addEventListener('change', renderKeyboard);
    }
});

// Physical keyboard binding
document.addEventListener('keydown', function(e) {
    var modal = document.getElementById('keyboardModal');
    var isKeyboardOpen = modal && modal.classList.contains('flex');

    // Escape to close keyboard
    if (e.key === 'Escape' && isKeyboardOpen) {
        closeKeyboard();
        return;
    }

    // Alt+K to toggle keyboard
    if (e.altKey && e.key.toLowerCase() === 'k') {
        e.preventDefault();
        if (isKeyboardOpen) {
            closeKeyboard();
        } else {
            // open for current focused input if any
            var ae = document.activeElement;
            if (ae && (ae.tagName === 'INPUT' || ae.tagName === 'TEXTAREA')) {
                openKeyboard({ stopPropagation: function() {} }, ae);
            }
        }
        return;
    }

    // Only process keyboard input when keyboard modal is open
    if (!currentInputField || !isKeyboardOpen) {
        return;
    }

    var key = e.key;

    // Handle Shift key state
    if (key === 'Shift') {
        e.preventDefault();
        shiftKeyActive = true;
        renderKeyboard();
        return;
    }

    // Handle special keys
    if (key === ' ') {
        e.preventDefault();
        insertCharacter({ key: ' ' }, false);
        highlightVirtualKey('space');
        return;
    }

    if (key === 'Backspace') {
        e.preventDefault();
        if (e.shiftKey) {
            deleteWord();
        } else {
            deleteCharacter();
        }
        highlightVirtualKey('backspace');
        return;
    }

    if (key === 'CapsLock') {
        e.preventDefault();
        toggleCapsLock();
        highlightVirtualKey('caps');
        return;
    }

    // Handle regular characters
    var charCode = key.charCodeAt(0);
    if ((charCode >= 48 && charCode <= 57) || // Numbers
        (charCode >= 65 && charCode <= 90) || // Uppercase letters
        (charCode >= 97 && charCode <= 122) || // Lowercase letters
        [',', '.', '/', ';', "'", '[', ']', '\\', '-', '=', '`'].includes(key)) {
        
        e.preventDefault();
        
        var layout = getActiveLayout();
        var keyObj = findKeyInLayout(layout, key);
        
        if (keyObj) {
            insertCharacter(keyObj, e.shiftKey);
            highlightVirtualKey(keyObj.key);
        }
    }
});

// Handle Shift key release
document.addEventListener('keyup', function(e) {
    if (e.key === 'Shift' && shiftKeyActive) {
        shiftKeyActive = false;
        var modal = document.getElementById('keyboardModal');
        if (modal && modal.classList.contains('flex')) {
            renderKeyboard();
        }
    }
});

function getActiveLayout() {
    var selector = document.getElementById('keyboardLangSelector');
    var mode = selector ? selector.value : 'target';
    var lang = mode === 'target' ? (selectedLanguage || 'de') : 'english';
    return keyboardLayouts[lang] || keyboardLayouts['de'];
}

function findKeyInLayout(layout, key) {
    for (var i = 0; i < layout.rows.length; i++) {
        for (var j = 0; j < layout.rows[i].length; j++) {
            if (layout.rows[i][j].key === key || layout.rows[i][j].key.toLowerCase() === key.toLowerCase()) {
                return layout.rows[i][j];
            }
        }
    }
    return null;
}

function deleteWord() {
    if (!currentInputField) return;

    var start = currentInputField.selectionStart;
    var text = currentInputField.value;

    // Move back to start of word
    while (start > 0 && text[start - 1] !== ' ') {
        start--;
    }

    var newText = text.substring(0, start) + text.substring(currentInputField.selectionEnd);
    currentInputField.value = newText;
    currentInputField.selectionStart = currentInputField.selectionEnd = start;
    currentInputField.focus();

    var event = new Event('input', { bubbles: true });
    currentInputField.dispatchEvent(event);
}

function highlightVirtualKey(keyId) {
    var keyEl = document.getElementById('vkey_' + keyId);
    if (!keyEl) return;

    keyEl.style.background = '#667eea';
    keyEl.style.color = 'white';
    keyEl.style.borderColor = '#667eea';

    setTimeout(function() {
        keyEl.style.background = '';
        keyEl.style.color = '';
        keyEl.style.borderColor = '';
    }, 100);
}

// Auto-focus first input field after form submission
document.addEventListener('DOMContentLoaded', function() {
    var forms = document.querySelectorAll('form[method="post"]');
    forms.forEach(function(form) {
        form.addEventListener('submit', function() {
            setTimeout(function() {
                var firstInput = form.querySelector('input:not([type="hidden"]), textarea:not([style*="display: none"])');
                if (firstInput) {
                    firstInput.focus();
                }
            }, 100);
        });
    });
});
