using FluentValidation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator(){
        RuleFor(p => p.Descripcion).NotNull().NotEmpty();
        RuleFor(P => P.Precio).NotNull().NotEmpty().GreaterThan(0);
    }
}