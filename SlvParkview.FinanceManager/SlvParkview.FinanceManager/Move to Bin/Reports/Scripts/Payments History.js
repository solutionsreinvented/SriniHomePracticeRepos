// #region Content
var jsonContents = {
  "ReportedMonth": "September",
  "ReportedYear": "2022",
  "Filter": "All",
  "Payments": [
    {
      "FlatNumber": "C118",
      "ReceivedOn": "05-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C120",
      "ReceivedOn": "05-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C323",
      "ReceivedOn": "05-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C018",
      "ReceivedOn": "11-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C020",
      "ReceivedOn": "12-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C021",
      "ReceivedOn": "12-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C022",
      "ReceivedOn": "12-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C320",
      "ReceivedOn": "13-09-2022",
      "Amount": "2,800.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C017",
      "ReceivedOn": "14-09-2022",
      "Amount": "3,000.0",
      "Mode": "Online",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C221",
      "ReceivedOn": "14-09-2022",
      "Amount": "2,800.0",
      "Mode": "Online",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C223",
      "ReceivedOn": "14-09-2022",
      "Amount": "1,400.0",
      "Mode": "Cash",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C224",
      "ReceivedOn": "15-09-2022",
      "Amount": "2,800.0",
      "Mode": "Online",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C124",
      "ReceivedOn": "17-09-2022",
      "Amount": "2,800.0",
      "Mode": "Online",
      "Category": "Maintenance"
    },
    {
      "FlatNumber": "C120",
      "ReceivedOn": "18-09-2022",
      "Amount": "2,800.0",
      "Mode": "Online",
      "Category": "Maintenance"
    }
  ],
  "TotalPayment": "38,000.0",
  "GeneratedOn": "20 Sep 2022",
  "ApartmentName": "SLV Parkview Apartment",
  "DocumentTitle": "Payments History"
};
// #endregion

// #region Retrieve Table Elements
var tableTitle = document.querySelector(".table-title");
var tableBody = document.querySelector(".table-body");
// #endregion

// #region Process Data
populatePayments();
// #endregion

// #region Populate Payments

function populatePayments() {

  var payments = jsonContents.Payments;

  var endOfData = "";
  tableBody.innerHTML = "";

  document.title = jsonContents.DocumentTitle

  // tableTitle.innerHTML = "Payments Received for " + jsonContents.ReportedMonth + " " + jsonContents.ReportedYear;

  tableTitle.innerHTML = jsonContents.DocumentTitle

  if (payments.length > 0) {

    for (i = 0; i < payments.length; i++) {

      tableBody.innerHTML += `
            <tr class="transaction-row">
              <td class="paid-on">${payments[i].ReceivedOn}</td>
              <td class="flat-number">${payments[i].FlatNumber}</td>
              <td class="owner-name">${payments[i].OwnerName}</td>
              <td class="amount">₹${payments[i].Amount}</td>
              <td class="payment-mode">${payments[i].Mode}</td>
              <td class="payment-category">${payments[i].Category}</td>
            </tr>
            `;

    }

    endOfData = "**** End of Report ****";

  } else {

    endOfData = "**** No Records Found ****";

  }

  tableBody.innerHTML += `<tr class="total-collection">
            <td colspan="5" class="total-collection-desc">Total Payment Received:</td>
            <td colspan="1" class="total-collection-amount">₹${jsonContents.TotalPayment}</td>
        </tr>`;

  tableBody.innerHTML += `<tr class="end-of-data">
            <td colspan="6">${endOfData}</td>
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
