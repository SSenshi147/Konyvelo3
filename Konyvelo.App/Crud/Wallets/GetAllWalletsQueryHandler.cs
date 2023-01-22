using CsharpGoodies.MediatrCrud.QueryHandlers;
using CsharpGoodies.Repo;
using Konyvelo.Logic.Domain;

namespace Konyvelo.Logic.Crud.Wallets;

public class GetAllWalletsQueryHandler : GetAllQueryHandler<Wallet, GetAllWalletsQuery>
{
    public GetAllWalletsQueryHandler(ICrudRepo<Wallet> repository) : base(repository)
    {
    }
}
