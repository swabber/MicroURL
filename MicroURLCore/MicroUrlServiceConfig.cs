using MicroURLCore.ShortIdGenerators;
using MicroURLData;

namespace MicroURLCore
{
    public record MicroUrlServiceConfig(string Domain, string User, DbContext Context, ShortIdGenerator Generator, IStatistics Statistics);
}
