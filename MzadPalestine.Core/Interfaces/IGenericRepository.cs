using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.Core.Interfaces;

public interface IGenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    // Additional generic repository methods can be added here
}