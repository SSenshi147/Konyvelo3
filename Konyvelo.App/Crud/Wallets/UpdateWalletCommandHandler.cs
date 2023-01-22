using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Wallets;

public class UpdateWalletCommandHandler : UpdateEntityCommandHandler<Wallet, UpdateWalletCommand>
{
    public UpdateWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
