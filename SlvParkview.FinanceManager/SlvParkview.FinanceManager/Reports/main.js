const outstandings = [
    {
        "TransactionDate": "2022-09-05T00:00:00",
        "PaymentAmount": "2000.00",
        "PaymentMode": "Cash",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "2000.00",
        "ExpenseCategory": "Maintenance",
        "Outstanding": "2800.00"
    },
    {
        "TransactionDate": "2022-09-05T00:00:00",
        "PaymentAmount": "2000.00",
        "PaymentMode": "Online",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "-",
        "ExpenseCategory": "-",
        "Outstanding": "800.00"
    },
    {
        "TransactionDate": "2022-09-05T00:00:00",
        "PaymentAmount": "2000.00",
        "PaymentMode": "Online",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "-",
        "ExpenseCategory": "-",
        "Outstanding": "-1200.00"
    },
    {
        "TransactionDate": "2022-09-06T00:00:00",
        "PaymentAmount": "-",
        "PaymentMode": "-",
        "PaymentCategory": "-",
        "ExpenseAmount": "2000.00",
        "ExpenseCategory": "Maintenance",
        "Outstanding": "800.00"
    },
    {
        "TransactionDate": "2022-09-06T00:00:00",
        "PaymentAmount": "-",
        "PaymentMode": "-",
        "PaymentCategory": "-",
        "ExpenseAmount": "2000.00",
        "ExpenseCategory": "Maintenance",
        "Outstanding": "2800.00"
    },
    {
        "TransactionDate": "2022-09-12T00:00:00",
        "PaymentAmount": "2000.00",
        "PaymentMode": "Cash",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "-",
        "ExpenseCategory": "-",
        "Outstanding": "800.00"
    },
    {
        "TransactionDate": "2022-09-13T00:00:00",
        "PaymentAmount": "-",
        "PaymentMode": "-",
        "PaymentCategory": "-",
        "ExpenseAmount": "2800.00",
        "ExpenseCategory": "Maintenance",
        "Outstanding": "3600.00"
    },
    {
        "TransactionDate": "2022-09-15T00:00:00",
        "PaymentAmount": "2800.00",
        "PaymentMode": "Online",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "-",
        "ExpenseCategory": "-",
        "Outstanding": "800.00"
    },
    {
        "TransactionDate": "2022-09-15T00:00:00",
        "PaymentAmount": "2800.00",
        "PaymentMode": "Cash",
        "PaymentCategory": "Maintenance",
        "ExpenseAmount": "-",
        "ExpenseCategory": "-",
        "Outstanding": "-2000.00"
    }
]

for (i = 0; i < outstandings.length; i++) {
    console.log(outstandings[0].TransactionDate);
}


function getFilename(fullPath) {
    return fullPath.replace(/^.*[\\\/]/, '');
}


