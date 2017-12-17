
namespace SJ.GameServer.DataAccess.Redis.Repository
{
    public abstract class RedisRepositoryBase<TEntity> where TEntity : class
    {
        public virtual bool Add(string key, TEntity entity)
        {
            return RedisCacheClient.cacheClient.Add<TEntity>(key, entity);
        }

        public virtual TEntity Find(string key)
        {
            return RedisCacheClient.cacheClient.Get<TEntity>(key);
        }

        public virtual bool Update(string key, TEntity entity)
        {
            return RedisCacheClient.cacheClient.Add<TEntity>(key, entity);
        }
    }
}
