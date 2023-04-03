// #region Content
var jsonContents = {
  FlatInfoCollection: [
    {
      Number: "217",
      Description: "C217",
      OwnedBy: "Srinivasa Rao Masanam",
      CoOwnedBy: "-",
      TenantName: "-",
      ResidentType: "Owner",
      AccountOpenedOn: "01 Sep 2022",
      DateSpecified: "20 Sep 2022",
      OpeningBalance: "2,800.00",
      OutstandingOnSpecifiedDate: "2,800.00",
      CurrentOutstanding: "2,800.00",
    },
  ],
  TotalOutstanding: "2,28,717.0",
  GeneratedOn: "20 Sep 2022",
  ApartmentName: "SLV Parkview Apartment",
  DocumentTitle: "Block Outstandings",
};
// #endregion

// #region Retrieve Table Elements
var tableHeader = document.querySelector(".table-header");
var tableBody = document.querySelector(".table-body");
// #endregion

// #region Data Required for Calculations
var outstandings = jsonContents.FlatInfoCollection.map(outstandingOnly);
var maxOutstanding = Math.max(...outstandings);

var penalties = jsonContents.FlatInfoCollection.map(penaltyOnly);
var maxPenalty = Math.max(...penalties);
// #endregion

document.querySelector(".outstanding-header").innerHTML = jsonContents.ShowPenaltiesOnly ? "Total Penalty" : "Outstanding";

populateOutstandings();

// #region Populate Outstandings

function populateOutstandings() {
  var endOfData = "";
  var flatInfoCollection = jsonContents.FlatInfoCollection;

  tableBody.innerHTML = "";

  if (flatInfoCollection.length > 0) {
    ///var reportTill = jsonContents.FlatInfoCollection[0].DateSpecified;
    document.title = jsonContents.DocumentTitle;
    document.querySelector(".table-title").innerHTML =
      jsonContents.DocumentTitle;

    for (i = 0; i < flatInfoCollection.length; i++) {
      tableBody.innerHTML += `
            <tr>
                <td>${flatInfoCollection[i].Description}</td>
                <td>${flatInfoCollection[i].OwnedBy}</td>
                <td>${flatInfoCollection[i].CoOwnedBy}</td>
                <td>${flatInfoCollection[i].TenantName}</td>
                <td>${flatInfoCollection[i].ResidentType}</td>
                <td>${formatAmount(jsonContents.ShowPenaltiesOnly ? flatInfoCollection[i].PenaltyTillSpecifiedDate :
        flatInfoCollection[i].OutstandingOnSpecifiedDate)}</td>
            </tr>`;
    }

    endOfData = "**** End of Report ****";
  } else {
    endOfData = "**** No Records Found ****";
  }

  tableBody.innerHTML += `<tr class="total-outstanding" >
            <td colspan="5" class="total-outstanding-desc">Total ${jsonContents.ShowPenaltiesOnly ? "penalty:" : "outstanding amount:"} </td>
            <td colspan="1" class="total-outstanding-amount">${formatAmount(jsonContents.ShowPenaltiesOnly ? jsonContents.TotalPenalty : jsonContents.TotalOutstanding)}</td>
        </tr> `;

  tableBody.innerHTML += `<tr class="end-of-data">
            <td colspan="6">${endOfData}</td>
        </tr>`;
}

// #endregion

// #region Limits for Cell Backgrounds
var noFocus = 0.0;
var lowFocus = 20.0;
var normalFocus = 40.0;
var aboveNormalFocus = 60.0;
var mediumFocus = 80.0;
// #endregion

if (jsonContents.Filter == 0) {
  changeElementBackground();
}

// #region Change Element Background

function changeElementBackground() {
  var flatCount = jsonContents.FlatInfoCollection.length;

  for (index = 0; index < flatCount; index++) {
    var tableRow = tableBody.children[index];

    var nameCell = tableRow.children[1];
    var outstandingCell = tableRow.children[tableRow.children.length - 1];

    var currentItemPercentage = getPercentage(
      jsonContents.ShowPenaltiesOnly ? penalties[index] : outstandings[index],
      jsonContents.ShowPenaltiesOnly ? maxPenalty : maxOutstanding
    );
    var focusClass;

    if (currentItemPercentage <= noFocus) {
      focusClass = "no-focus";
    } else if (currentItemPercentage <= lowFocus) {
      focusClass = "low-focus";
    } else if (currentItemPercentage <= normalFocus) {
      focusClass = "normal-focus";
    } else if (currentItemPercentage <= aboveNormalFocus) {
      focusClass = "above-normal-focus";
    } else if (currentItemPercentage <= mediumFocus) {
      focusClass = "medium-focus";
    } else {
      focusClass = "high-focus";
    }
    nameCell.classList.add(focusClass);
    outstandingCell.classList.add(focusClass);
  }
}

// #endregion

// #region Helper Functions for Cell Coloring

function outstandingOnly(flatInfo) {
  return parseFloat(stripNumberFormatting(flatInfo.OutstandingOnSpecifiedDate));
}

function penaltyOnly(flatInfo) {
  return parseFloat(stripNumberFormatting(flatInfo.PenaltyTillSpecifiedDate));
}

function getPercentage(current, max) {
  return (current / max) * 100;
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

function stripNumberFormatting(formattedValue) {
  if (formattedValue.includes("(") && formattedValue.includes(")")) {
    formattedValue = "-" + formattedValue.replace("(", "").replace(")", "");
  }
  if (formattedValue.includes(",")) {
    formattedValue = formattedValue.replace(",", "");
  }
  return formattedValue;
}

function formatAmount(amount) {
  if (amount.includes("(")) {
    return amount.replace("(", "(₹");
  }
  else {
    return "₹" + amount;
  }
}

// #endregion
