using FluentValidation;

public class ProductValidator : AbstractValidator<Producto>
{
    public ProductValidator(){
        RuleFor(p => p.Descripcion).NotNull().NotEmpty();
        RuleFor(P => P.Precio).NotNull().NotEmpty().GreaterThan(0);
    }
}