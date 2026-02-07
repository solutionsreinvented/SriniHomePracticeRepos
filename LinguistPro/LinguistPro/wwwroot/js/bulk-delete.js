// Bulk Delete Helper Functions

function toggleAllCheckboxes(checkboxes) {
    if (checkboxes.length === 0) return;
    
    // Find the select-all checkbox
    let selectAllCheckbox = null;
    if (checkboxes[0].className.includes('vocab')) {
        selectAllCheckbox = document.getElementById('selectAllVocab');
    } else if (checkboxes[0].className.includes('verb')) {
        selectAllCheckbox = document.getElementById('selectAllVerbs');
    } else if (checkboxes[0].className.includes('item')) {
        selectAllCheckbox = document.getElementById('selectAllItems');
    }
    
    if (!selectAllCheckbox) return;
    
    const isChecked = selectAllCheckbox.checked;
    checkboxes.forEach(cb => cb.checked = isChecked);
    
    if (checkboxes[0].className.includes('vocab')) {
        updateVocabBulkDeleteState();
    } else if (checkboxes[0].className.includes('verb')) {
        updateVerbBulkDeleteState();
    } else if (checkboxes[0].className.includes('item')) {
        updateItemBulkDeleteState();
    }
}

function updateVocabBulkDeleteState() {
    const checkboxes = document.querySelectorAll('.vocab-checkbox:checked');
    const btn = document.getElementById('bulkDeleteVocabBtn');
    const count = document.getElementById('vocabSelectedCount');
    
    if (btn) {
        btn.disabled = checkboxes.length === 0;
        console.log('Vocab: ' + checkboxes.length + ' selected, button disabled=' + btn.disabled);
    }
    if (count) {
        count.textContent = checkboxes.length + ' selected';
    }
}

function updateVerbBulkDeleteState() {
    const checkboxes = document.querySelectorAll('.verb-checkbox:checked');
    const btn = document.getElementById('bulkDeleteVerbBtn');
    const count = document.getElementById('verbSelectedCount');
    
    if (btn) {
        btn.disabled = checkboxes.length === 0;
    }
    if (count) {
        count.textContent = checkboxes.length + ' selected';
    }
}

function updateItemBulkDeleteState() {
    const checkboxes = document.querySelectorAll('.item-checkbox:checked');
    const btn = document.getElementById('bulkDeleteItemBtn');
    const count = document.getElementById('itemSelectedCount');
    
    if (btn) {
        btn.disabled = checkboxes.length === 0;
    }
    if (count) {
        count.textContent = checkboxes.length + ' selected';
    }
}

function deleteBulkVocabHandler() {
    const checkboxes = document.querySelectorAll('.vocab-checkbox:checked');
    if (checkboxes.length === 0) {
        alert('Please select items to delete');
        return;
    }
    
    if (confirm(`Delete ${checkboxes.length} vocabulary items? This cannot be undone.`)) {
        const ids = Array.from(checkboxes).map(cb => cb.dataset.id);
        submitBulkDelete('/Index?handler=BulkDeleteVocabulary', ids);
    }
}

function deleteBulkVerbHandler() {
    const checkboxes = document.querySelectorAll('.verb-checkbox:checked');
    if (checkboxes.length === 0) {
        alert('Please select items to delete');
        return;
    }
    
    if (confirm(`Delete ${checkboxes.length} verbs? This cannot be undone.`)) {
        const ids = Array.from(checkboxes).map(cb => cb.dataset.id);
        submitBulkDelete('/Index?handler=BulkDeleteVerbs', ids);
    }
}

function deleteBulkItemHandler() {
    const checkboxes = document.querySelectorAll('.item-checkbox:checked');
    if (checkboxes.length === 0) {
        alert('Please select items to delete');
        return;
    }
    
    if (confirm(`Delete ${checkboxes.length} items? This cannot be undone.`)) {
        const ids = Array.from(checkboxes).map(cb => cb.dataset.id);
        submitBulkDelete('/Index?handler=BulkDeleteLanguageItems', ids);
    }
}

function submitBulkDelete(url, ids) {
    const form = document.createElement('form');
    form.method = 'post';
    form.action = url;
    
    // Add CSRF token
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    if (token) {
        const tokenInput = document.createElement('input');
        tokenInput.type = 'hidden';
        tokenInput.name = '__RequestVerificationToken';
        tokenInput.value = token.value;
        form.appendChild(tokenInput);
    }

    // Add selected IDs
    ids.forEach(id => {
        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'selectedIds';
        input.value = id;
        form.appendChild(input);
    });

    document.body.appendChild(form);
    form.submit();
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    // Add change event listeners to all checkboxes
    document.querySelectorAll('.vocab-checkbox').forEach(cb => {
        cb.addEventListener('change', updateVocabBulkDeleteState);
    });
    document.querySelectorAll('.verb-checkbox').forEach(cb => {
        cb.addEventListener('change', updateVerbBulkDeleteState);
    });
    document.querySelectorAll('.item-checkbox').forEach(cb => {
        cb.addEventListener('change', updateItemBulkDeleteState);
    });
});
