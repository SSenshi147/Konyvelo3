using CsharpGoodies.Domain;

namespace Konyvelo.Logic.Domain;
public class Currency : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public List<Wallet> Wallets { get; set; } = new();
}
