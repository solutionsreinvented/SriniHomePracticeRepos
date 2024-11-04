
using ReInvented.Shared.Base;
using ReInvented.Shared.Interfaces;

namespace DevDrive.Models
{
    public class ProcessResult : Result, IResult
    {
        #region Parameterized Constructor

        public ProcessResult(bool success, string message) : base(message, success)
        {

        }

        #endregion
    }
}
