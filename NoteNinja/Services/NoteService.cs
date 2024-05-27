using System.Collections.Generic;
using System.Threading.Tasks;
using NoteNinja.Models;
using NoteNinja.Repositories;

namespace NoteNinja.Services
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;

        public NoteService(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(string userId)
        {
            return await _noteRepository.GetAllAsync(n => n.UserId == userId);
        }

        public async Task<Note> GetByIdAsync(string id)
        {
            return await _noteRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Note note)
        {
            await _noteRepository.CreateAsync(note);
        }

        public async Task UpdateAsync(string id, Note note)
        {
            await _noteRepository.UpdateAsync(id, note);
        }

        public async Task DeleteAsync(string id)
        {
            await _noteRepository.DeleteAsync(id);
        }
    }
}
