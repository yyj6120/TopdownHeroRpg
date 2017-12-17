using System;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace SJ.GameServer.DataAccess.Redis
{
    // sagolboss : TODO : 차후 StackExchange.Redis.Extensions 사용하는 것을 삭제하고 StackExchange.Redis을 사용한다.
    public static class RedisCacheClient
    {
        // sagolboss : connect 관련 처리는 https://docs.microsoft.com/ko-kr/azure/redis-cache/cache-dotnet-how-to-use-azure-redis-cache 문서를 참고한다.
        public static StackExchangeRedisCacheClient cacheClient
        {
            get
            {
                if (_cacheClient == null)
                {
                    _cacheClient = new Lazy<StackExchangeRedisCacheClient>(() =>
                    {
                        return new StackExchangeRedisCacheClient(new NewtonsoftSerializer());
                    });
                }
                return _cacheClient.Value;
            }
        }

        private static Lazy<StackExchangeRedisCacheClient> _cacheClient;
    }
}
