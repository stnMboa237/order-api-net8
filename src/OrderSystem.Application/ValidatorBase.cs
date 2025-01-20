using FluentValidation;

namespace OrderSystem.Application
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>
    {
        // Você pode adicionar comportamento comum para seus validadores aqui
        // Exemplo de regra comum que pode ser usada em todos os validadores
        protected ValidatorBase()
        {
        }
    }
}