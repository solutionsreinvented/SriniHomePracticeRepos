// ===== CUSTOM DROPDOWN FUNCTIONALITY =====

class CustomDropdown {
    constructor(selectElement) {
        this.selectElement = selectElement;
        this.wrapper = selectElement.parentElement;
        this.isOpen = false;
        this.init();
    }

    init() {
        // Create custom dropdown structure
        this.createDropdownUI();
        this.attachEventListeners();
    }

    createDropdownUI() {
        // Create button
        const button = document.createElement('button');
        button.type = 'button';
        button.className = 'custom-select-button';

        // Create button content
        const textContainer = document.createElement('div');
        textContainer.className = 'custom-select-text';

        const iconSpan = document.createElement('span');
        iconSpan.className = 'custom-select-icon';

        const labelSpan = document.createElement('span');
        labelSpan.className = 'custom-select-label';

        const chevron = document.createElement('span');
        chevron.className = 'custom-select-chevron';
        chevron.innerHTML = '▲';

        textContainer.appendChild(iconSpan);
        textContainer.appendChild(labelSpan);
        button.appendChild(textContainer);
        button.appendChild(chevron);

        // Store references
        this.button = button;
        this.iconSpan = iconSpan;
        this.labelSpan = labelSpan;

        // Create dropdown container
        const dropdown = document.createElement('div');
        dropdown.className = 'custom-select-dropdown';

        // Populate options
        const options = this.selectElement.querySelectorAll('option');
        options.forEach((option, index) => {
            if (index === 0 && !option.value) return; // Skip placeholder option

            const optionDiv = document.createElement('div');
            optionDiv.className = 'custom-select-option';
            optionDiv.dataset.value = option.value;
            optionDiv.dataset.icon = option.dataset.icon || '🌍';

            const iconEl = document.createElement('span');
            iconEl.className = 'custom-select-option-icon';
            iconEl.textContent = option.dataset.icon || '🌍';

            const textEl = document.createElement('span');
            textEl.className = 'custom-select-option-text';
            textEl.textContent = option.textContent;

            optionDiv.appendChild(iconEl);
            optionDiv.appendChild(textEl);
            dropdown.appendChild(optionDiv);
        });

        this.dropdown = dropdown;

        // Insert into DOM
        this.wrapper.insertBefore(button, this.selectElement);
        this.wrapper.insertBefore(dropdown, this.selectElement);

        // Update initial button text
        this.updateButtonText();
    }

    attachEventListeners() {
        // Toggle dropdown
        this.button.addEventListener('click', (e) => {
            e.preventDefault();
            this.toggleDropdown();
        });

        // Handle option selection
        this.dropdown.querySelectorAll('.custom-select-option').forEach(option => {
            option.addEventListener('click', (e) => {
                e.preventDefault();
                this.selectOption(option);
            });
        });

        // Close dropdown when clicking outside
        document.addEventListener('click', (e) => {
            if (!this.wrapper.contains(e.target) && this.isOpen) {
                this.closeDropdown();
            }
        });

        // Keyboard navigation
        this.button.addEventListener('keydown', (e) => {
            if (e.key === 'ArrowDown') {
                e.preventDefault();
                this.openDropdown();
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                this.closeDropdown();
            } else if (e.key === 'Escape') {
                this.closeDropdown();
            }
        });

        this.dropdown.addEventListener('keydown', (e) => {
            const options = Array.from(this.dropdown.querySelectorAll('.custom-select-option'));
            const currentIndex = options.findIndex(opt => opt.classList.contains('selected'));

            if (e.key === 'ArrowDown') {
                e.preventDefault();
                const nextIndex = currentIndex + 1 < options.length ? currentIndex + 1 : 0;
                this.selectOption(options[nextIndex]);
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                const prevIndex = currentIndex - 1 >= 0 ? currentIndex - 1 : options.length - 1;
                this.selectOption(options[prevIndex]);
            } else if (e.key === 'Enter') {
                e.preventDefault();
                this.closeDropdown();
            }
        });
    }

    toggleDropdown() {
        if (this.isOpen) {
            this.closeDropdown();
        } else {
            this.openDropdown();
        }
    }

    openDropdown() {
        this.dropdown.classList.add('active');
        this.button.classList.add('active');
        this.isOpen = true;

        // Focus first option for keyboard navigation
        const firstOption = this.dropdown.querySelector('.custom-select-option');
        if (firstOption) {
            firstOption.focus();
        }
    }

    closeDropdown() {
        this.dropdown.classList.remove('active');
        this.button.classList.remove('active');
        this.isOpen = false;
        this.button.focus();
    }

    selectOption(optionElement) {
        // Remove selection from all options
        this.dropdown.querySelectorAll('.custom-select-option').forEach(opt => {
            opt.classList.remove('selected');
        });

        // Select the clicked option
        optionElement.classList.add('selected');

        // Update select element
        const value = optionElement.dataset.value;
        this.selectElement.value = value;

        // Update button text
        this.updateButtonText();

        // Trigger change event
        const event = new Event('change', { bubbles: true });
        this.selectElement.dispatchEvent(event);

        // Close dropdown
        this.closeDropdown();
    }

    updateButtonText() {
        const selectedOption = this.selectElement.selectedOptions[0];
        if (selectedOption) {
            this.iconSpan.textContent = selectedOption.dataset.icon || '🌍';
            this.labelSpan.textContent = selectedOption.textContent;

            // Mark option as selected
            const optionDiv = this.dropdown.querySelector(`[data-value="${selectedOption.value}"]`);
            if (optionDiv) {
                this.dropdown.querySelectorAll('.custom-select-option').forEach(opt => {
                    opt.classList.remove('selected');
                });
                optionDiv.classList.add('selected');
            }
        }
    }
}

// Initialize all custom dropdowns on page load
document.addEventListener('DOMContentLoaded', () => {
    const selectElements = document.querySelectorAll('select.custom-select');
    selectElements.forEach(select => {
        new CustomDropdown(select);
    });
});
