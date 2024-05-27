using System.Collections.Generic;
using System.Threading.Tasks;
using NoteNinja.Models;

namespace NoteNinja.Services
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetNotesByUserIdAsync(string userId);
        Task<Note> GetByIdAsync(string id);
        Task CreateAsync(Note note);
        Task UpdateAsync(string id, Note note);
        Task DeleteAsync(string id);
    }
}
