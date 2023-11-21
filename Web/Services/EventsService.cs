using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Web.Services
{
    public interface IEventsService
    {
        public IEnumerable<Event> GetEvents();
    }

    public class EventsService : IEventsService
    {
        private readonly DataContext _context;

        public EventsService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetEvents()
        {
           return _context.Events;
        }
    }
}