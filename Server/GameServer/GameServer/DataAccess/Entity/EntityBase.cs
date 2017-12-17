using System;
using System.Collections.Generic;
using AutoMapper;

namespace SJ.GameServer.DataAccess.Entity
{
    public abstract class EntityBase
    {
        public TVo Map<TVo>()
        {
            return Mapper.Map<TVo>(this);
        }
    }
}
