// TODO:
// Implement reading data from a local json file istead of hard coding
// the json contents below.

// #region Content
var jsonContents = {
  FlatInfo: {
    Number: "217",
    Description: "C217",
    OwnedBy: "Srinivasa Rao Masanam",
    CoOwnedBy: "-",
    TenantName: "-",
    ResidentType: "Owner",
    AccountOpenedOn: "01 September 2022",
    DateSpecified: "13 September 2022",
    OpeningBalance: "2,800.00",
    OutstandingOnSpecifiedDate: "3,600.00",
    CurrentOutstanding: "(79,600.00)",
  },
  Transactions: [
    {
      TransactionDate: "05 September 2022",
      PaymentAmount: "2,000.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "2,000.00",
      ExpenseCategory: "Maintenance",
      Outstanding: "2,800.00",
    },
    {
      TransactionDate: "05 September 2022",
      PaymentAmount: "2,000.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "800.00",
    },
    {
      TransactionDate: "05 September 2022",
      PaymentAmount: "2,000.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(1,200.00)",
    },
    {
      TransactionDate: "06 September 2022",
      PaymentAmount: "-",
      PaymentMode: "-",
      PaymentCategory: "-",
      ExpenseAmount: "2,000.00",
      ExpenseCategory: "Maintenance",
      Outstanding: "800.00",
    },
    {
      TransactionDate: "06 September 2022",
      PaymentAmount: "-",
      PaymentMode: "-",
      PaymentCategory: "-",
      ExpenseAmount: "2,000.00",
      ExpenseCategory: "Maintenance",
      Outstanding: "2,800.00",
    },
    {
      TransactionDate: "12 September 2022",
      PaymentAmount: "2,000.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "800.00",
    },
    {
      TransactionDate: "13 September 2022",
      PaymentAmount: "-",
      PaymentMode: "-",
      PaymentCategory: "-",
      ExpenseAmount: "2,800.00",
      ExpenseCategory: "Maintenance",
      Outstanding: "3,600.00",
    },
    {
      TransactionDate: "15 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "800.00",
    },
    {
      TransactionDate: "15 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(2,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,000.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(4,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(6,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(9,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(12,400.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(15,200.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(18,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(20,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(23,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cheque",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(26,400.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(29,200.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Cash",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(32,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(34,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(37,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(40,400.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(43,200.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(46,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(48,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(51,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(54,400.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(57,200.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(60,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(62,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(65,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(68,400.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(71,200.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(74,000.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(76,800.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
    {
      TransactionDate: "16 September 2022",
      PaymentAmount: "2,800.00",
      PaymentMode: "Online",
      PaymentCategory: "Maintenance",
      ExpenseAmount: "-",
      ExpenseCategory: "-",
      Outstanding: "(79,600.00)",
    },
  ],
};
// #endregion

// #region Retrieve Tables
var flatInfoTable = document.querySelector(".flat-info");
var summaryTable = document.querySelector(".transactions-table");
// #endregion

// #region Process Data
updateFlatInformation();
updateTransactionDetails();
// #endregion

// #region Update Flat Information
function updateFlatInformation() {
  var flat = jsonContents.FlatInfo;

  flatInfoTable.querySelector(".flat-desc").innerHTML = flat.Description;
  flatInfoTable.querySelector(".resident-type").innerHTML = flat.ResidentType;
  flatInfoTable.querySelector(".owner-name").innerHTML = flat.OwnedBy;
  flatInfoTable.querySelector(".opening-date").innerHTML = flat.AccountOpenedOn;
  flatInfoTable.querySelector(".coowner-name").innerHTML = flat.CoOwnedBy;
  flatInfoTable.querySelector(".opening-balance").innerHTML =
    flat.OpeningBalance;
  flatInfoTable.querySelector(".tenant-name").innerHTML = flat.TenantName;
  flatInfoTable.querySelector(".current-outstanding").innerHTML =
    flat.CurrentOutstanding;

  document.title = "Transaction Summary (" + flat.Description + ")";
}
// #endregion

// #region Update Transaction Details
function updateTransactionDetails() {
  var endOfData = "";
  var transactions = jsonContents.Transactions;
  var summaryTableBody = summaryTable.querySelector(".summary-body");
  summaryTableBody.innerHTML = "";

  if (transactions.length > 0) {
    for (i = 0; i < transactions.length; i++) {
      summaryTableBody.innerHTML += `
            <tr class="transaction-row">
                <td>${transactions[i].TransactionDate.toString()}</td>
                <td>${transactions[i].PaymentAmount}</td>
                <td>${transactions[i].PaymentMode}</td>
                <td>${transactions[i].PaymentCategory}</td>
                <td>${transactions[i].ExpenseAmount}</td>
                <td>${transactions[i].ExpenseCategory}</td>
                <td>${transactions[i].Outstanding}</td>
            </tr>
            `;
    }

    endOfData = "**** End of Data ****";
  } else {
    endOfData = "**** No Records Found ****";
  }

  summaryTableBody.innerHTML += `<tr class="end-of-data">
            <td colspan="7">${endOfData}</td>
        </tr>`;
}
// #endregion

// #region Helper Functions
function getFilename(fullPath) {
  return fullPath.replace(/^.*[\\\/]/, "");
}

function getCurrentDirectory() {
  var location = window.location.pathname;
  return location.substring(0, location.lastIndexOf("/"));
}
// #endregion
