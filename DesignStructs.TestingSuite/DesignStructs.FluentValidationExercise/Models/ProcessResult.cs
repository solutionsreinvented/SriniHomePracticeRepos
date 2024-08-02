namespace DesignStructs.FluentValidationExercise.Models
{
    public abstract class Result : IResult
    {
        #region Default Constructor

        public Result()
        {

        }

        #endregion

        #region Parameterized Constructor

        public Result(bool hasSucceeded, string message)
        {
            HasSucceeded = hasSucceeded;
            Message = message;
        }

        #endregion

        #region Public Properties

        public bool HasSucceeded { get; set; }

        public string Message { get; set; }

        #endregion
    }

    public class ProcessResult : Result
    {
        #region Parameterized Constructor

        public ProcessResult(bool hasSucceeded, string message) : base(hasSucceeded, message)
        {

        }

        #endregion
    }
}
