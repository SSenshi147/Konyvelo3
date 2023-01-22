using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Wallets;

public class DeleteWalletCommandHandler : DeleteEntityCommandHandler<Wallet, DeleteWalletCommand>
{
    public DeleteWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
