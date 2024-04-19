using Microsoft.EntityFrameworkCore;

namespace cp2.Services;

public class EncurtadorUrlService
{
    public const int QtdeCharsEncurtadorNoLink = 7;
    private const string Alfabeto = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
    private readonly Random _random = new();
    private readonly EncurtadorDbContext _dbContext;

    public EncurtadorUrlService(EncurtadorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GerarCodigoUnico()
    {
        var charsCodigo = new char[QtdeCharsEncurtadorNoLink];

        while (true)
        {
            for (var i = 0; i < QtdeCharsEncurtadorNoLink; i++)
            {
                int randomIntex = _random.Next(Alfabeto.Length - 1);

                charsCodigo[i] = Alfabeto[randomIntex];
            }

            var codigo = new string(charsCodigo);

            if (!await _dbContext.EncurtadorUrls.AnyAsync(s => s.Codigo == codigo))
            {
                return codigo;
            }
        }
    }
}