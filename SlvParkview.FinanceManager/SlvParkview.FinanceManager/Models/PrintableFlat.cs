using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;

using System;

namespace SlvParkview.FinanceManager.Models
{
    public class PrintableFlat : PropertyStore
    {
        #region Public Properties

        /// <summary>
        /// Flat number.
        /// </summary>
        public string Number { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Provides the complete description of the flat.
        /// </summary>
        public string Description { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Primary owner of the flat.
        /// </summary>
        public string OwnedBy { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Name of the joint owner if the flat is owned by more than person.
        /// </summary>
        public string CoOwnedBy { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Name of the tenant if the flat is not occupied by the owner(s).
        /// </summary>
        public string TenantName { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Indicates whether flat is occupied by the owner(s) or a tenant.
        /// </summary>
        public string ResidentType { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Date on which the flat account is started.
        /// </summary>
        public string AccountOpenedOn { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Date for which the results to be calculated.
        /// </summary>
        public string DateSpecified { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Outstanding balance pending as of the date of opening this account.
        /// </summary>
        public string OpeningBalance { get => Get<string>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        /// <summary>
        /// Outstanding balance pending as on the selected date.
        /// </summary>
        public string OutstandingOnSpecifiedDate { get => Get<string>(); set => Set(value); }
        /// <summary>
        /// Outstanding balance pending calculated till date.
        /// </summary>
        public string CurrentOutstanding { get => Get<string>(); set => Set(value); }

        #endregion

        public static PrintableFlat MapFrom(Flat flat)
        {
            string dateFormat = "dd MMMM yyyy";

            return new PrintableFlat()
            {
                AccountOpenedOn = flat.AccountOpenedOn.ToString(dateFormat),
                CoOwnedBy = flat.CoOwnedBy ?? "-",
                CurrentOutstanding = flat.CurrentOutstanding.FormatNumber("N2"),
                DateSpecified = flat.DateSpecified.ToString(dateFormat),
                Description = flat.Description,
                Number = flat.Number.ToString(),
                OpeningBalance = flat.OpeningBalance.FormatNumber("N2"),
                OutstandingOnSpecifiedDate = flat.OutstandingOnSpecifiedDate.FormatNumber("N2"),
                OwnedBy = flat.OwnedBy,
                ResidentType = flat.ResidentType.ToString(),
                TenantName = flat.TenantName ?? "-"
            };
        }

    }

}
