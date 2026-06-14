using System;
namespace Nofshonit.Infrastructure.EF
{
    /// <summary>
    /// Context factory interface
    /// </summary>
    public interface IClubContextFactory
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        IDbContext DbContext { get; }

   
    }
}
