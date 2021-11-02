// ReSharper disable All

namespace SimpleDomain.Tests
{
    using Xunit;

    public partial class V2
    {
        public class Collections
        {
            [CollectionDefinition(nameof(V2) + nameof(SqlServer))]
            public class SqlServer : ICollectionFixture<SqlServerEnv<Definition>>
            {
            }


            [CollectionDefinition(nameof(V2) + nameof(SqLite))]
            public class SqLite : ICollectionFixture<SqLiteEnv<Definition>>
            {
            }
        }
    }
}