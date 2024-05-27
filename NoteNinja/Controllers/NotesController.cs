using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteNinja.Models;
using NoteNinja.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NoteNinja.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INoteService noteService, ILogger<NotesController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var notes = await _noteService.GetNotesByUserIdAsync(User.Identity.Name);
            return View(notes);
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Content("Test route is working!");
        }

        [HttpGet]
        public async Task<IActionResult> GetNoteContent(string id)
        {
            var note = await _noteService.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Json(new { title = note.Title, content = note.Content });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNote([FromBody] Note note)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingNote = await _noteService.GetByIdAsync(note.Id);
                    if (existingNote == null)
                    {
                        return NotFound();
                    }

                    existingNote.Title = note.Title;
                    existingNote.Content = note.Content;
                    existingNote.UserId = note.UserId ?? User.Identity.Name; // Ensure UserId is set correctly
                    existingNote.TagId = note.TagId ?? "default"; // Assign a default or existing TagId if not provided

                    await _noteService.UpdateAsync(existingNote.Id, existingNote);
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving note");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNote([FromForm] string title, [FromForm] string content, [FromForm] string tagId, [FromForm] string userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var note = new Note
                    {
                        Title = title,
                        Content = content,
                        UserId = userId ?? User.Identity.Name,
                        TagId = tagId ?? "default"
                    };
                    await _noteService.CreateAsync(note);
                    return Json(new { id = note.Id, title = note.Title, tagId = note.TagId });
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating note");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNote(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Note ID is required.");
                }

                await _noteService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting note");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
