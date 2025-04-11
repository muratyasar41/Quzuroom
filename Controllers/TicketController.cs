using Microsoft.AspNetCore.Mvc;
using CompanyManagementSystem.Web.Models;
using CompanyManagementSystem.Web.Services;

namespace CompanyManagementSystem.Web.Controllers
{
    public class TicketController : Controller
    {
        private static List<Ticket> _tickets = new List<Ticket>();
        private readonly VersionControlService _versionService;

        public TicketController(VersionControlService versionService)
        {
            _versionService = versionService;
        }

        public IActionResult Index()
        {
            return View(_tickets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = _tickets.Count + 1;
                ticket.CreatedDate = DateTime.Now;
                ticket.Status = "Open";
                _tickets.Add(ticket);

                // Versiyon oluştur
                _versionService.CreateVersion(
                    "Ticket",
                    ticket.Id,
                    ticket,
                    "Ticket created",
                    "System" // Gerçek uygulamada kullanıcı adı gelecek
                );

                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        public IActionResult Details(int id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            // Versiyonları al
            ViewBag.Versions = _versionService.GetVersions("Ticket", id);
            return View(ticket);
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var oldStatus = ticket.Status;
            ticket.Status = newStatus;

            // Versiyon oluştur
            _versionService.CreateVersion(
                "Ticket",
                ticket.Id,
                ticket,
                $"Status changed from {oldStatus} to {newStatus}",
                "System" // Gerçek uygulamada kullanıcı adı gelecek
            );

            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult ViewVersion(int id, string versionNumber)
        {
            var version = _versionService.GetVersion("Ticket", id, versionNumber);
            if (version == null)
            {
                return NotFound();
            }

            var ticket = System.Text.Json.JsonSerializer.Deserialize<Ticket>(version.CurrentState);
            ViewBag.VersionInfo = version;
            return View("Details", ticket);
        }
    }
} 