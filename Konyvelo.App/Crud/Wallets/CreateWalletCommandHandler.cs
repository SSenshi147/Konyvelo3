using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Wallets;
public class CreateWalletCommandHandler : CreateEntityCommandHandler<Wallet, CreateWalletCommand>
{
    public CreateWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
