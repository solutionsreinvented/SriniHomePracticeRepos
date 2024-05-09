using FluentValidation;

namespace DesignStructs.FluentValidationExercise.Extensions
{
    public static class IRuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> NotNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string propertyName)
        {
            return ruleBuilder.NotNull().WithMessage($"{propertyName} shall not be null.");
        }
        public static IRuleBuilderOptions<T, TProperty> NotEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string propertyName)
        {
            return ruleBuilder.NotEmpty().WithMessage($"Invalid {propertyName}. The {propertyName} is either empty or contains only whitespace characters.");
            //return ruleBuilder.NotEmpty().WithMessage($"{propertyName} shall not be null, an empty string, whitespace, an empty collection or the default value for the type (for example, 0 for integers but null for nullable integers)");
        }
    }
}
