function createTableRow(classList) {
    let trow = document.createElement('tr');
    classList.forEach(c => { trow.classList.add(c) });

    return trow;
}
function createTableDataCell(classList, innerHtml, rowspan = undefined, colspan = undefined) {

    let td = document.createElement('td');

    if (rowspan !== undefined) {
        td.rowspan = rowspan;
    }
    if (colspan !== undefined) {
        td.colspan = colspan;
    }

    classList.forEach(c => { td.classList.add(c) });
    td.innerHTML = innerHtml;

    return td;
}

function getDescriptionItemContent(item, itemIndex, itemsCount, revisionCode) {

    let trow = createTableDataCell(['no-page-break-inside']);

    if (itemIndex == 0) {
        const tdRevCode = createTableDataCell(['align-center'], revisionCode, itemsCount, undefined);
        trow.appendChild(tdRevCode);
    }
    const tdSection = createTableDataCell(['align-left'], item.Description, undefined, undefined);
    const tdDescription = createTableDataCell(['align-left'], item.Description, undefined, undefined);

    trow.appendChild(tdSection);
    trow.appendChild(tdDescription);

    return trow;
}