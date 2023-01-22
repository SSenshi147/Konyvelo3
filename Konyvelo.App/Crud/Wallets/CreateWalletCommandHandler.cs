using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Wallets;
public class CreateWalletCommandHandler : CreateEntityCommandHandler<Wallet, CreateWalletCommand>
{
    public CreateWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
