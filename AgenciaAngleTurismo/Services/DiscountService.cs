namespace AgenciaTurismo.Services
{
    public delegate decimal CalculateDelegate(DateOnly dataNascimento, decimal precoOriginal);

    public class DiscountService
    {
        public static decimal ApplySeniorDiscount(DateOnly dataNascimento, decimal precoOriginal)
        {
            var hoje = DateOnly.FromDateTime(DateTime.Now);
            int idade = hoje.Year - dataNascimento.Year;
            if (hoje < dataNascimento.AddYears(idade)) idade--;

            return idade >= 65 ? precoOriginal * 0.9m : precoOriginal;
        }

    }
}
