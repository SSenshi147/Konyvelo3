using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Wallets;

public class DeleteWalletCommandHandler : DeleteEntityCommandHandler<Wallet, DeleteWalletCommand>
{
    public DeleteWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
