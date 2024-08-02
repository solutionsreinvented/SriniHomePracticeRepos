namespace DesignStructs.FluentValidationExercise.Models
{
    public interface IResult
    {
        bool HasSucceeded { get; set; }

        string Message { get; set; }
    }
}