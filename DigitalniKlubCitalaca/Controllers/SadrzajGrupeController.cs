using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalniKlubCitalaca.Data;
using DigitalniKlubCitalaca.Models;

namespace DigitalniKlubCitalaca.Controllers
{
    public class SadrzajGrupeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SadrzajGrupeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sadrzaji = _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa);

            return View(await sadrzaji.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);

            if (sadrzajGrupe == null) return NotFound();

            return View(sadrzajGrupe);
        }

        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Korisnici.ToList(), "Id", "Email");
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe.ToList(), "GrupaId", "GrupaId");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SadrzajId,AutorId,GrupaId,Naslov,Opis,Link,TipSadrzaja,StatusSadrzaja")] SadrzajGrupe sadrzajGrupe)
        {
            ModelState.Remove("DatumObjave");
            ModelState.Remove("Autor");
            ModelState.Remove("CitalackaGrupa");

            sadrzajGrupe.DatumObjave = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(sadrzajGrupe);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.Korisnici.ToList(), "Id", "Email", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe.ToList(), "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);

            return View(sadrzajGrupe);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sadrzajGrupe = await _context.SadrzajiGrupe.FindAsync(id);

            if (sadrzajGrupe == null) return NotFound();

            ViewData["AutorId"] = new SelectList(_context.Korisnici.ToList(), "Id", "Email", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe.ToList(), "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);

            return View(sadrzajGrupe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SadrzajId,AutorId,GrupaId,DatumObjave,Naslov,Opis,Link,TipSadrzaja,StatusSadrzaja")] SadrzajGrupe sadrzajGrupe)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("CitalackaGrupa");

            if (id != sadrzajGrupe.SadrzajId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sadrzajGrupe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SadrzajGrupeExists(sadrzajGrupe.SadrzajId))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.Korisnici.ToList(), "Id", "Email", sadrzajGrupe.AutorId);
            ViewData["GrupaId"] = new SelectList(_context.CitalackeGrupe.ToList(), "GrupaId", "GrupaId", sadrzajGrupe.GrupaId);

            return View(sadrzajGrupe);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sadrzajGrupe = await _context.SadrzajiGrupe
                .Include(s => s.Autor)
                .Include(s => s.CitalackaGrupa)
                .FirstOrDefaultAsync(m => m.SadrzajId == id);

            if (sadrzajGrupe == null) return NotFound();

            return View(sadrzajGrupe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sadrzajGrupe = await _context.SadrzajiGrupe.FindAsync(id);

            if (sadrzajGrupe != null)
            {
                _context.SadrzajiGrupe.Remove(sadrzajGrupe);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SadrzajGrupeExists(int id)
        {
            return _context.SadrzajiGrupe.Any(e => e.SadrzajId == id);
        }
    }
}