// ===== CUSTOM DROPDOWN - PRODUCTION IMPLEMENTATION =====

class CustomDropdown {
    constructor(selectElement) {
        console.log('[CustomDropdown] Constructor called with:', selectElement?.name);

        if (!selectElement) {
            console.error('[CustomDropdown] Select element is null');
            return;
        }

        this.selectElement = selectElement;
        this.wrapper = selectElement.closest('.custom-select-wrapper');
        this.isOpen = false;

        console.log('[CustomDropdown] Wrapper found:', !!this.wrapper);

        if (!this.wrapper) {
            console.error('[CustomDropdown] Wrapper .custom-select-wrapper not found for:', selectElement.name);
            console.error('[CustomDropdown] Parent elements:', selectElement.parentElement?.className);
            return;
        }

        try {
            console.log('[CustomDropdown] Starting init...');
            this.init();
            console.log('[CustomDropdown] ✓ Initialized successfully for:', selectElement.name);
            console.log('[CustomDropdown] ✓ Button created:', !!this.button);
            console.log('[CustomDropdown] ✓ Dropdown created:', !!this.dropdown);
        } catch (err) {
            console.error('[CustomDropdown] Init error:', err);
            console.error('[CustomDropdown] Stack:', err.stack);
        }
    }

    init() {
        console.log('[CustomDropdown] init() starting...');

        // Create UI elements
        this.createUI();
        console.log('[CustomDropdown] createUI() completed');

        // Attach event listeners
        this.attachEventListeners();
        console.log('[CustomDropdown] attachEventListeners() completed');

        // Update display with initial value
        this.updateDisplay();
        console.log('[CustomDropdown] updateDisplay() completed');
    }

    createUI() {
        console.log('[CustomDropdown] createUI() starting...');

        // Create button
        this.button = document.createElement('button');
        this.button.type = 'button';
        this.button.className = 'custom-select-button';
        this.button.setAttribute('aria-haspopup', 'listbox');
        this.button.setAttribute('aria-expanded', 'false');
        console.log('[CustomDropdown] Button element created');

        // Create button text container
        const textContainer = document.createElement('div');
        textContainer.className = 'custom-select-text';

        this.iconSpan = document.createElement('span');
        this.iconSpan.className = 'custom-select-icon flag-image';

        this.labelSpan = document.createElement('span');
        this.labelSpan.className = 'custom-select-label';
        this.labelSpan.textContent = '-- Select a Language --';

        textContainer.appendChild(this.iconSpan);
        textContainer.appendChild(this.labelSpan);
        console.log('[CustomDropdown] Text container created');

        // Create chevron
        const chevron = document.createElement('span');
        chevron.className = 'custom-select-chevron';
        chevron.textContent = '▲';

        this.button.appendChild(textContainer);
        this.button.appendChild(chevron);
        console.log('[CustomDropdown] Button fully assembled');

        // Create dropdown container
        this.dropdown = document.createElement('div');
        this.dropdown.className = 'custom-select-dropdown';
        this.dropdown.setAttribute('role', 'listbox');
        console.log('[CustomDropdown] Dropdown container created');

        // Populate options from select element
        const options = this.selectElement.querySelectorAll('option');
        console.log('[CustomDropdown] Found options:', options.length);

        options.forEach((option, index) => {
            // Skip empty placeholder option
            if (index === 0 && !option.value) {
                console.log('[CustomDropdown] Skipping placeholder option');
                return;
            }

            const optionDiv = document.createElement('div');
            optionDiv.className = 'custom-select-option';
            optionDiv.dataset.value = option.value;
            optionDiv.setAttribute('role', 'option');
            optionDiv.setAttribute('aria-selected', 'false');

            const iconEl = document.createElement('span');
            iconEl.className = 'custom-select-option-icon';
            const iconPath = option.dataset.icon || '/images/flags/de.svg';
            iconEl.style.backgroundImage = `url('${iconPath}')`;

            const textEl = document.createElement('span');
            textEl.className = 'custom-select-option-text';
            textEl.textContent = option.textContent;

            optionDiv.appendChild(iconEl);
            optionDiv.appendChild(textEl);

            // Click handler for option
            optionDiv.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation();
                this.selectOption(optionDiv);
            });

            this.dropdown.appendChild(optionDiv);
        });
        console.log('[CustomDropdown] All options added to dropdown');

        // Insert UI into DOM
        console.log('[CustomDropdown] Inserting UI into DOM...');
        console.log('[CustomDropdown] Wrapper children before insert:', this.wrapper.children.length);

        try {
            this.wrapper.insertBefore(this.button, this.selectElement);
            console.log('[CustomDropdown] ✓ Button inserted');
            this.wrapper.insertBefore(this.dropdown, this.selectElement);
            console.log('[CustomDropdown] ✓ Dropdown inserted');
        } catch (err) {
            console.error('[CustomDropdown] Error inserting into DOM:', err);
            throw err;
        }

        console.log('[CustomDropdown] Wrapper children after insert:', this.wrapper.children.length);
    }

    attachEventListeners() {
        // Button click
        this.button.addEventListener('click', (e) => {
            e.preventDefault();
            e.stopPropagation();
            this.toggle();
        });

        // Close on outside click
        document.addEventListener('click', (e) => {
            if (!this.wrapper.contains(e.target) && this.isOpen) {
                this.close();
            }
        });

        // Keyboard support
        this.button.addEventListener('keydown', (e) => {
            if (e.key === 'ArrowDown' || e.key === ' ') {
                e.preventDefault();
                this.open();
            }
        });

        // Handle select element changes (if changed programmatically)
        this.selectElement.addEventListener('change', () => {
            this.updateDisplay();
        });
    }

    updateDisplay() {
        const selected = this.selectElement.selectedOptions[0];
        if (selected && selected.value) {
            const iconPath = selected.dataset.icon || '/images/flags/de.svg';
            this.iconSpan.style.backgroundImage = `url('${iconPath}')`;
            this.labelSpan.textContent = selected.textContent;

            // Update option styling
            this.dropdown.querySelectorAll('.custom-select-option').forEach(opt => {
                opt.classList.remove('selected');
                opt.setAttribute('aria-selected', 'false');
            });

            const selectedOpt = this.dropdown.querySelector(`[data-value="${selected.value}"]`);
            if (selectedOpt) {
                selectedOpt.classList.add('selected');
                selectedOpt.setAttribute('aria-selected', 'true');
            }
        } else {
            this.labelSpan.textContent = '-- Select a Language --';
            this.iconSpan.style.backgroundImage = `url('/images/flags/de.svg')`;
        }
    }

    toggle() {
        this.isOpen ? this.close() : this.open();
    }

    open() {
        this.dropdown.classList.add('active');
        this.button.classList.add('active');
        this.button.setAttribute('aria-expanded', 'true');
        this.isOpen = true;
    }

    close() {
        this.dropdown.classList.remove('active');
        this.button.classList.remove('active');
        this.button.setAttribute('aria-expanded', 'false');
        this.isOpen = false;
    }

    selectOption(optionElement) {
        const value = optionElement.dataset.value;
        
        // Update hidden select
        this.selectElement.value = value;

        // Trigger change event
        const event = new Event('change', { bubbles: true });
        this.selectElement.dispatchEvent(event);

        // Update display
        this.updateDisplay();

        // Close dropdown
        this.close();
    }
}

// ===== AUTO-INITIALIZATION =====

function initializeAllDropdowns() {
    console.log('═══════════════════════════════════════════════════');
    console.log('[CustomDropdown] INITIALIZING ALL DROPDOWNS...');
    console.log('═══════════════════════════════════════════════════');

    const selects = document.querySelectorAll('select.custom-select');
    console.log('[CustomDropdown] Found select elements:', selects.length);

    if (selects.length === 0) {
        console.warn('[CustomDropdown] NO SELECT ELEMENTS FOUND!');
        console.log('[CustomDropdown] All select elements on page:');
        document.querySelectorAll('select').forEach((s, i) => {
            console.log(`  ${i}. ${s.name} - classes: ${s.className}`);
        });
        return;
    }

    let initialized = 0;
    selects.forEach((select, index) => {
        console.log(`\n[CustomDropdown] Processing select ${index + 1}/${selects.length}`);
        console.log('[CustomDropdown] Select name:', select.name);
        console.log('[CustomDropdown] Select classes:', select.className);
        console.log('[CustomDropdown] Already initialized?:', select.dataset.customDropdownInit);

        if (!select.dataset.customDropdownInit) {
            console.log('[CustomDropdown] → Creating new CustomDropdown instance');
            new CustomDropdown(select);
            select.dataset.customDropdownInit = '1';
            initialized++;
        } else {
            console.log('[CustomDropdown] → Already initialized, skipping');
        }
    });

    console.log(`\n[CustomDropdown] INITIALIZATION COMPLETE - ${initialized}/${selects.length} initialized`);
    console.log('[CustomDropdown] Checking final state:');
    console.log('[CustomDropdown] Custom buttons now on page:', document.querySelectorAll('.custom-select-button').length);
    console.log('[CustomDropdown] Custom dropdowns now on page:', document.querySelectorAll('.custom-select-dropdown').length);
    console.log('═══════════════════════════════════════════════════\n');
}

// Initialize when DOM is ready
console.log('[CustomDropdown] Script loaded, waiting for DOM...');
if (document.readyState === 'loading') {
    console.log('[CustomDropdown] DOM still loading, attaching DOMContentLoaded listener');
    document.addEventListener('DOMContentLoaded', () => {
        console.log('[CustomDropdown] DOMContentLoaded fired');
        setTimeout(initializeAllDropdowns, 100);
    });
} else {
    // DOM already loaded
    console.log('[CustomDropdown] DOM already loaded, initializing immediately');
    setTimeout(initializeAllDropdowns, 100);
}

// Expose for manual initialization if needed
window.CustomDropdown = CustomDropdown;
window.initializeAllDropdowns = initializeAllDropdowns;

console.log('[CustomDropdown] Script ready. Call window.initializeAllDropdowns() to manually trigger initialization');
