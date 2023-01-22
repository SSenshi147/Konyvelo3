using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.App.Domain;

namespace Konyvelo.App.Crud.Wallets;

public class GetAllWalletsQueryHandler : GetAllQueryHandler<Wallet, GetAllWalletsQuery>
{
    public GetAllWalletsQueryHandler(ICrudRepo<Wallet> repository) : base(repository)
    {
    }
}
