using System.Collections.Generic;
using GRM.Domain;

namespace GRM.Repository
{
    public interface IMusicContractsRepository
    {
        List<MusicContract> GetMusicContracts();
    }
}