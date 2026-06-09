using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Controllers
{
    public class CitalackaGrupaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitalackaGrupaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CitalackeGrupe.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
{
    if (id == null) return NotFound();

    var citalackaGrupa = await _context.CitalackeGrupe
        .FirstOrDefaultAsync(m => m.GrupaId == id);

    if (citalackaGrupa == null) return NotFound();

    ViewBag.Objave = await _context.SadrzajiGrupe
        .Include(s => s.Autor)
        .Where(s => s.GrupaId == id)
        .OrderByDescending(s => s.DatumObjave)
        .ToListAsync();

    return View(citalackaGrupa);
}
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe")] CitalackaGrupa citalackaGrupa)
        {
            ModelState.Remove("DatumKreiranja");

            citalackaGrupa.DatumKreiranja = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(citalackaGrupa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(citalackaGrupa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);

            if (citalackaGrupa == null) return NotFound();

            return View(citalackaGrupa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrupaId,Naziv,Opis,Zanr,Regija,Grad,MaksimalanBrojClanova,TipGrupe,DatumKreiranja")] CitalackaGrupa citalackaGrupa)
        {
            if (id != citalackaGrupa.GrupaId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citalackaGrupa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitalackaGrupaExists(citalackaGrupa.GrupaId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(citalackaGrupa);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var citalackaGrupa = await _context.CitalackeGrupe
                .FirstOrDefaultAsync(m => m.GrupaId == id);

            if (citalackaGrupa == null) return NotFound();

            return View(citalackaGrupa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var citalackaGrupa = await _context.CitalackeGrupe.FindAsync(id);

            if (citalackaGrupa != null)
            {
                _context.CitalackeGrupe.Remove(citalackaGrupa);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CitalackaGrupaExists(int id)
        {
            return _context.CitalackeGrupe.Any(e => e.GrupaId == id);
        }
    }
}