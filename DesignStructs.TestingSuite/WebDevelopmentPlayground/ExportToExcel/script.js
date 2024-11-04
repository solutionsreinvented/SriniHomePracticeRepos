document.getElementById("exportButton").addEventListener("click", exportToExcel);

async function exportToExcel() {
    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet("Exported Data");

    // 1. Add Tables to the Sheet
    const tables = document.querySelectorAll("table");
    tables.forEach((table, tableIndex) => {
        const rows = Array.from(table.querySelectorAll("tr")).map(row =>
            Array.from(row.querySelectorAll("th, td")).map(cell => cell.innerText)
        );

        rows.forEach((rowData, rowIndex) => {
            const excelRow = sheet.getRow(rowIndex + 1 + tableIndex * (rows.length + 2));
            rowData.forEach((cellData, cellIndex) => {
                excelRow.getCell(cellIndex + 1).value = cellData;
            });
            excelRow.commit();
        });
    });

    // 2. Add SVG Images to the Workbook
    const svgs = document.querySelectorAll("svg");
    for (let svgIndex = 0; svgIndex < svgs.length; svgIndex++) {
        const svg = svgs[svgIndex];
        const svgData = new XMLSerializer().serializeToString(svg);
        const svgBase64 = `data:image/svg+xml;base64,${btoa(svgData)}`;

        const imageId = workbook.addImage({
            base64: svgBase64,
            extension: "png",
        });

        // Position image at cell B10 (for example)
        sheet.addImage(imageId, {
            tl: { col: 1, row: svgIndex * 10 + 1 },
            ext: { width: 300, height: 200 }, // Set dimensions as needed
        });
    }

    // 3. Trigger download
    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], { type: "application/octet-stream" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = "exported_data.xlsx";
    a.click();
    URL.revokeObjectURL(url);
}
