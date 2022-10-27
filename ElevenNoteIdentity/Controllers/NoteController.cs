using ElevenNote.Models;
using ElevenNote.Models.Note;
using ElevenNote.Services.Notes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace ElevenNoteIdentity.Controllers
{
    [Authorize]
    public class NoteController :  Controller
    { public readonly NoteService _service;
        public NoteController(NoteService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {



            var svc = CreateNoteService();
            var model = svc.GetNotes();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var svc = CreateNoteService();

            if (svc.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            };
            ModelState.AddModelError("", "Note could not be created.");

            return View(model);
        }
        public ActionResult Details(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);

            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var detail = service.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content
                };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);

            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();

            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted";

            return RedirectToAction("Index");
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(GetUserId());
            var service = new NoteService(userId);
            return service;
        }

        private string GetUserId()
        {
            string userIdClaim = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;
            if (userIdClaim == null) return null;
            return userIdClaim;
        }
           
        
    }
}
