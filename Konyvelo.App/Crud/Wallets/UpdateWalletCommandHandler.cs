using CsharpGoodies.MediatrCrud.CommandHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Wallets;

public class UpdateWalletCommandHandler : UpdateEntityCommandHandler<Wallet, UpdateWalletCommand>
{
    public UpdateWalletCommandHandler(ICrudRepo<Wallet> crudRepo) : base(crudRepo)
    {
    }
}
