using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;


namespace HomeMealTaste.Data.Implement
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        private readonly HomeMealTasteContext _context;
        public SessionRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }
    }
}
