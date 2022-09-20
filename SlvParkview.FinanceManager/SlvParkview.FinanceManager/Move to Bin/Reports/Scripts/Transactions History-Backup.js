// #region Content
var jsonContents = {
  "FlatInfo": {
    "Number": "120",
    "Description": "C120",
    "OwnedBy": "Shivanand Chawhan",
    "CoOwnedBy": "-",
    "TenantName": "-",
    "ResidentType": "Owner",
    "AccountOpenedOn": "01 Sep 2022",
    "DateSpecified": "13 Sep 2022",
    "OpeningBalance": "5,600.0",
    "OutstandingOnSpecifiedDate": "2,800.0",
    "CurrentOutstanding": "0.0"
  },
  "Transactions": [
    {
      "TransactionDate": "05 September 2022",
      "PaymentAmount": "2,800.0",
      "PaymentMode": "Cash",
      "PaymentCategory": "Maintenance",
      "ExpenseAmount": "-",
      "ExpenseCategory": "-",
      "Outstanding": "2,800.0"
    },
    {
      "TransactionDate": "18 September 2022",
      "PaymentAmount": "2,800.0",
      "PaymentMode": "Online",
      "PaymentCategory": "Maintenance",
      "ExpenseAmount": "-",
      "ExpenseCategory": "-",
      "Outstanding": "0.0"
    }
  ],
  "GeneratedOn": "20 Sep 2022",
  "ApartmentName": "SLV Parkview Apartment"
};
// #endregion

// #region Retrieve Tables
var flatInfoTable = document.querySelector(".flat-info");
var summaryTable = document.querySelector(".transactions-table");
// #endregion

// #region Process Data
populateFlatInformation();
populateTransactionDetails();
// #endregion

// #region Populate Flat Information

function populateFlatInformation() {
  var flat = jsonContents.FlatInfo;

  flatInfoTable.querySelector(".flat-desc").innerHTML = flat.Description;
  flatInfoTable.querySelector(".resident-type").innerHTML = flat.ResidentType;
  flatInfoTable.querySelector(".owner-name").innerHTML = flat.OwnedBy;
  flatInfoTable.querySelector(".opening-date").innerHTML = flat.AccountOpenedOn;
  flatInfoTable.querySelector(".coowner-name").innerHTML = flat.CoOwnedBy;
  flatInfoTable.querySelector(".opening-balance").innerHTML = flat.OpeningBalance;
  flatInfoTable.querySelector(".tenant-name").innerHTML = flat.TenantName;
  flatInfoTable.querySelector(".current-outstanding").innerHTML = flat.CurrentOutstanding;

  document.title = "Transaction Summary (" + flat.Description + ")";
}

// #endregion

// #region Populate Transaction Details

function populateTransactionDetails() {

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
