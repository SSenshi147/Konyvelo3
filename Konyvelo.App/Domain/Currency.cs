using CsharpGoodies.Domain;

namespace Konyvelo.App.Domain;
public class Currency : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
