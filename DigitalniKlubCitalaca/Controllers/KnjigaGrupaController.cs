using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Controllers
{
    public class KnjigaGrupaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigaGrupaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KnjigaGrupa
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.KnjigeGrupe.Include(k => k.CitalackaGrupa).Include(k => k.Knjiga);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: KnjigaGrupa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjigaGrupa = await _context.KnjigeGrupe
                .Include(k => k.CitalackaGrupa)
                .Include(k => k.Knjiga)
                .FirstOrDefaultAsync(m => m.KnjigaGrupaId == id);
            if (knjigaGrupa == null)
            {
                return NotFound();
            }

            return View(knjigaGrupa);
        }

        // GET: KnjigaGrupa/Create
        public IActionResult Create()
        {
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId");
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId");
            return View();
        }

        // POST: KnjigaGrupa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KnjigaGrupaId,KnjigaId,GrupaId")] KnjigaGrupa knjigaGrupa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knjigaGrupa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", knjigaGrupa.GrupaId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", knjigaGrupa.KnjigaId);
            return View(knjigaGrupa);
        }

        // GET: KnjigaGrupa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjigaGrupa = await _context.KnjigeGrupe.FindAsync(id);
            if (knjigaGrupa == null)
            {
                return NotFound();
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", knjigaGrupa.GrupaId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", knjigaGrupa.KnjigaId);
            return View(knjigaGrupa);
        }

        // POST: KnjigaGrupa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KnjigaGrupaId,KnjigaId,GrupaId")] KnjigaGrupa knjigaGrupa)
        {
            if (id != knjigaGrupa.KnjigaGrupaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knjigaGrupa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaGrupaExists(knjigaGrupa.KnjigaGrupaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe, "GrupaId", "GrupaId", knjigaGrupa.GrupaId);
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", knjigaGrupa.KnjigaId);
            return View(knjigaGrupa);
        }

        // GET: KnjigaGrupa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjigaGrupa = await _context.KnjigeGrupe
                .Include(k => k.CitalackaGrupa)
                .Include(k => k.Knjiga)
                .FirstOrDefaultAsync(m => m.KnjigaGrupaId == id);
            if (knjigaGrupa == null)
            {
                return NotFound();
            }

            return View(knjigaGrupa);
        }

        // POST: KnjigaGrupa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjigaGrupa = await _context.KnjigeGrupe.FindAsync(id);
            if (knjigaGrupa != null)
            {
                _context.KnjigeGrupe.Remove(knjigaGrupa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnjigaGrupaExists(int id)
        {
            return _context.KnjigeGrupe.Any(e => e.KnjigaGrupaId == id);
        }
    }
}
